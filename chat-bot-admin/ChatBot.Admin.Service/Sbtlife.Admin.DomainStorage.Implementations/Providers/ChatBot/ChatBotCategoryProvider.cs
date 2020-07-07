using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.Common.Const.ChatBot;
using ChatBot.Admin.Common.Extensions;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Contexts;
using Um.Connect.Abstractions;

namespace ChatBot.Admin.DomainStorage.Providers.ChatBot
{
    internal class ChatBotCategoryProvider : ProviderChatBot, IChatBotCategoryProvider
    {

        private readonly IDateTimeService _dateTimeService;
        private readonly IChatInfoService _chatInfoService;
        private readonly IUser _user;


        public ChatBotCategoryProvider(ChatBotContext storage, IDateTimeService dateTimeService, IChatInfoService chatInfoService, IUser user)
            : base(storage)
        {
            _dateTimeService = dateTimeService;
            _chatInfoService = chatInfoService;
            _user = user;
        }

        public  void AddCategory(CategoryDto category)
        {
            var entity = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Response = category.Response,
                SetContext = category.SetContext,
                PartitionId = category.PartitionId,
                ChangedOn = category.ChangedOn,
                ChangedBy = category.ChangedBy,
                IsChanged = category.IsChanged,
                IsAdded = category.IsAdded,
                IsTest = category.IsTest,
                IsDisabled = category.IsDisabled,
                IsIneligible = category.IsIneligible ?? false,
                OriginId = category.OriginId ?? Guid.NewGuid(),
                RequiredRoster = category.RequiredRoster
            };
             Context.Categorys.Add(entity);
             Context.SaveChanges();
             UpdatePatterns(null, entity.Id, category.Patterns);
        }

        private string PrepareForRegex(string text)
        {
            var res = text;
            foreach (var c in "\\[]().*+=!?$^<>|")
            {
                res = res.Replace("" + c, "\\" + c);
            }
            return res;
        }

        private  void RenameXbtnLinks(string oldName, string newName, string changedBy)
        {
            var categ = new Regex($"(?<=<xbtn\\scategory=['\"])(?i){PrepareForRegex(oldName)}(?-i)(?=['\"]>.*?<\\/xbtn>)");

            var responses =  Context.Categorys.Where(x => (x.IsTest ?? false) && x.Response.Contains("<xbtn")).ToList();
            var isSave = false;
            foreach (var response in responses)
            {
                var match = categ.Match(response.Response);
                if (match.Success)
                {
                    response.Response = categ.Replace(response.Response, newName);
                    response.ChangedOn = _dateTimeService.SessionUtcNow.ToLocalTime();
                    response.ChangedBy = changedBy;
                    response.IsChanged = true;
                    isSave = true;
                }

            }
            if (isSave)
            {
                 Context.SaveChanges();
            }
        }

        public  void ModifyCategory(CategoryOptionalDto category)
        {
            var entity =  GetCategoryRaw(category.Id);

            if (category.IsChangedPatterns ?? false)
            {
                 UpdatePatterns(category.Id, entity.Id, category.Patterns);
            }

            if (category.Name != null && entity.Name != category.Name.Value)
            {
                 RenameXbtnLinks(entity.Name, category.Name.Value, entity.ChangedBy);
            }

            OptionalHelper.SafeUpdate(v => entity.Name = v, category.Name);
            OptionalHelper.SafeUpdate(v => entity.Response = v.Replace("= '", "='").Replace("= \"", "=\""), category.Response);
            OptionalHelper.SafeUpdate(v => entity.SetContext = v, category.SetContext);
            OptionalHelper.SafeUpdate(v => entity.PartitionId = v, category.PartitionId);
            entity.ChangedOn = category.ChangedOn;
            entity.ChangedBy = category.ChangedBy;
            entity.IsChanged = category.IsChanged;
            entity.IsAdded = category.IsAdded;
            entity.IsTest = category.IsTest;
            entity.IsDisabled = category.IsDisabled;
            entity.IsIneligible = category.IsIneligible ?? false;
            entity.RequiredRoster = category.RequiredRoster;

            category.Id = entity.Id;
             Context.SaveChanges();
        }

        public  void DeleteCategory(int id)
        {
            var pws =  (
                from p in Context.Patterns
                join pw in Context.PatternWordRels on p.Id equals pw.PatternId
                where p.CategoryId == id
                select pw).ToList();
            if (pws.Any())
            {
                Context.PatternWordRels.RemoveRange(pws);
                 Context.SaveChanges();
            }

            var ps =  Context.Patterns.Where(x => x.CategoryId == id).ToList();
            if (ps.Any())
            {
                Context.Patterns.RemoveRange(ps);
                 Context.SaveChanges();
            }

            var entity =  GetCategoryRaw(id);

            if (entity == null)
                throw new ArgumentException(nameof(id));

            Context.Categorys.Remove(entity);
             Context.SaveChanges();
        }

        public  bool CheckExistsAndNotDeleted(int id)
        {
            return  Context.Categorys.Any(l => l.Id == id);
        }


        public  bool CheckCategoryIsEditable(int id)
        {
            var cat =  Context.Categorys.SingleOrDefault(l => l.Id == id);
            return cat.IsTest ?? false;
        }

        public  bool CheckCaptionUnique(string caption)
        {
            return ! Context.Categorys.Any(x => (x.IsTest ?? false) && x.Name == caption)
                ;
        }


        private  Category GetCategoryRaw(int id)
        {
            return  Context.Categorys.SingleOrDefault(l => l.Id == id);
        }

        public  Category GetCategoryByName(string name, bool isTest)
        {
            return  Context.Categorys.SingleOrDefault(c => (c.IsTest ?? false) == isTest && c.Name == name);
        }


        public  string ValidateResponse(string response)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(response))
            {
                return "Не указан ответ";
            }
            if (response.Contains("<xbtn"))
            {
                var xbtn = new Regex("<xbtn\\s.*?>.*?<\\/xbtn>");
                var categ = new Regex("^<xbtn\\scategory=['\"](.*?)['\"]>.*?<\\/xbtn>$");
                var match = xbtn.Match(response);
                while (match.Success)
                {
                    if (!categ.IsMatch(match.Value))
                    {
                        errors.Add($"Кнопка {match.Value.Replace("<", "(").Replace(">", ")")}: не указана категория");
                    }
                    else
                    {
                        var matchCat = categ.Match(match.Value);
                        var catName = matchCat.Groups[1].Value;
                        if ( GetCategoryByName(catName, true) == null)
                            errors.Add($"Кнопка {match.Value.Replace("<", "(").Replace(">", ")")}: не найдена категория {catName}");
                    }

                    match = match.NextMatch();
                }
            }

            var orgReg = new Regex("<org>(.*?)</org>");
            var matchOrg = orgReg.Match(response);
            while (matchOrg.Success)
            {
                if (!_chatInfoService.OrgData().Contains(matchOrg.Groups[1].Value.ToLower()))
                {
                    errors.Add($"Неверное значение параметра {matchOrg.Value}");
                }

                matchOrg = matchOrg.NextMatch();
            }

            var xorgReg = new Regex("<xorg.*?\\scategory=['\"](.*?)['\"].*?>.*?</xorg>");
            var matchxOrg = xorgReg.Match(response);
            while (matchxOrg.Success)
            {
                if (!_chatInfoService.OrgData().Contains(matchxOrg.Groups[1].Value.ToLower()))
                {
                    errors.Add($"Неверное значение параметра {matchxOrg.Value}");
                }

                matchxOrg = matchxOrg.NextMatch();
            }

            return string.Join('\n', errors);
        }


        private  bool UpdatePatterns(int? categoryId, int catEntityId, List<PatternDto> patterns)
        {
            if (categoryId.HasValue)
            {
                var pws =  (
                    from p in Context.Patterns
                    join pw in Context.PatternWordRels on p.Id equals pw.PatternId
                    where p.CategoryId == categoryId.Value
                    select pw).ToList();
                if (pws.Any())
                {
                    Context.PatternWordRels.RemoveRange(pws);
                     Context.SaveChanges();
                }

                var ps =  (
                    from p in Context.Patterns
                    where p.CategoryId == categoryId.Value
                    select p).ToList();
                if (ps.Any())
                {
                    Context.Patterns.RemoveRange(ps);
                     Context.SaveChanges();
                }
            }

            foreach (var pat in patterns)
            {
                var patEntity = new Pattern()
                {
                    CategoryId = catEntityId,
                    Phrase = pat.Phrase,
                    Context = pat.Context,
                    OnlyContext = pat.OnlyContext,
                    WordCount = pat.Words.Count()
                };
                 Context.Patterns.Add(patEntity);
                 Context.SaveChanges();

                pat.Id = patEntity.Id;
                StoreWords(pat);
            }

            return true;
        }

        public  string ValidateDelete(int id)
        {
            var cat =  GetCategoryRaw(id);
            var categ = new Regex($"(?<=<xbtn\\scategory=['\"])(?i){PrepareForRegex(cat.Name)}(?-i)(?=['\"]>.*?<\\/xbtn>)");

            var responses =  Context.Categorys.Where(x => (x.IsTest ?? false) && x.Response.Contains("<xbtn")).ToList();
            foreach (var response in responses)
            {
                var match = categ.Match(response.Response);
                if (match.Success)
                {
                    return $"Категория используется в ответе '{response.Name}'";
                }
            }

            return null;
        }


        public  bool CheckAnyEditCategories()
        {
            var any =  Context.Categorys.Any(x => x.IsTest ?? false);
            return any;
        }

        public  void PublishCategories(Guid? partitionId, Guid? subPartId)
        {
            var paramSubPartition = new SqlParameter("@SubPartition", SqlDbType.UniqueIdentifier)
            {
                Value = (object)subPartId ?? DBNull.Value
            };
            var paramPartition = new SqlParameter("@Partition", SqlDbType.UniqueIdentifier)
            {
                Value = (object)partitionId ?? DBNull.Value
            };
             Context.Database.ExecuteSqlCommand("exec DeployConfig @SubPartition, @Partition", paramSubPartition, paramPartition);
        }

        public  void UnpublishCategories(Guid? partitionId, Guid? subPartId)
        {
            var paramSubPartition = new SqlParameter("@SubPartition", SqlDbType.UniqueIdentifier)
            {
                Value = (object)subPartId ?? DBNull.Value
            };
            var paramPartition = new SqlParameter("@Partition", SqlDbType.UniqueIdentifier)
            {
                Value = (object)partitionId ?? DBNull.Value
            };
             Context.Database.ExecuteSqlCommand("exec MakeTestCopy @SubPartition, @Partition", paramSubPartition, paramPartition);
        }

        public  List<WordListOutDto> GetCategoriesWords()
        {
            var req =  (
                    from wf in Context.WordForms
                    join w in Context.Words on wf.WordId equals w.Id
                    join pw in Context.PatternWordRels on w.Id equals pw.WordId
                    join p in Context.Patterns on pw.PatternId equals p.Id
                    join c in Context.Categorys on p.CategoryId equals c.Id
                    where !(c.IsTest ?? false) && !(c.IsDisabled ?? false)
                    group new { wf, p } by c into g
                    select new { g.Key.Name, g.Key.OriginId, ps = g.ToList() }).ToList();
            var res = req
                .Select(x => new WordListOutDto
                {
                    Category = x.Name,
                    CategoryOriginId = x.OriginId,
                    Patterns = x.ps.GroupBy(g => g.p)
                        .Select(g => new PatternLiteDto()
                        {
                            Phrase = g.Key.Phrase,
                            Words = g.Select(wf => wf.wf.Form).Distinct().ToList()
                        }).ToList()
                }).ToList();
            return res;
        }

        public  List<WordIndexDto> GetWordsForIndex()
        {
            var res = (
                from wf in Context.WordForms
                join w in Context.Words on wf.WordId equals w.Id
                join pw in Context.PatternWordRels on w.Id equals pw.WordId
                join p in Context.Patterns on pw.PatternId equals p.Id
                join c in Context.Categorys on p.CategoryId equals c.Id
                where !(c.IsTest ?? false) && !(c.IsDisabled ?? false)
                group new { wf, p } by wf.Form
                into g
                select new { word = g.Key, pattern = g.Select(x => x.p.Id).Distinct().ToList() });
            return  res.Select(x => new WordIndexDto()
            {
                Id = x.word,
                Pattern = x.pattern
            }).ToList();
        }


        public  CategoryDto GetByOriginId(Guid originId)
        {
            var entity =  (
                from c in Context.Categorys
                join p in Context.Partitions on c.PartitionId equals p.Id
                join pp in Context.Partitions on p.ParentId equals pp.Id
                where c.OriginId == originId && !(c.IsTest ?? false) && !(c.IsDisabled ?? false)
                select new {c, p, pp}).FirstOrDefault();
            var res = Mapper.Map<CategoryDto>(entity.c);
            if (res != null)
            {
                res.Partition = Mapper.Map<PartitionDto>(entity.p);
                res.UpperPartition = Mapper.Map<PartitionDto>(entity.pp);
                res.RequiredRosterName = _chatInfoService.RosterName(entity.c.RequiredRoster);
            }
            return res;
        }


        public  PatternDto GetTestPatternById(int patternId, int? categoryId)
        {
            var pat =  (
                from c in Context.Categorys
                join p in Context.Patterns on c.Id equals categoryId ?? p.CategoryId
                where p.Id == patternId && (c.IsTest ?? false)
                select p).FirstOrDefault();
            return Mapper.Map<PatternDto>(pat);
        }

        public  void DeletePattern(int patternId)
        {

            var pws =  (
                from pw in Context.PatternWordRels 
                where pw.PatternId == patternId
                select pw).ToList();
            if (pws.Any())
            {
                Context.PatternWordRels.RemoveRange(pws);
                 Context.SaveChanges();
            }

            var pat =  (
                from c in Context.Categorys
                join p in Context.Patterns on c.Id equals p.CategoryId
                where p.Id == patternId 
                select new { p, c }).FirstOrDefault();

            Context.Patterns.Remove(pat.p);
            
            pat.c.IsChanged = true;
            pat.c.ChangedBy = _user.SigmaLogin;
            pat.c.ChangedOn = _dateTimeService.SessionUtcNow.ToLocalTime();

             Context.SaveChanges();
        }

        public  void StorePattern(PatternDto pattern)
        {
            var oldCategoryId = 0;
            Pattern patEntity;
            if ((pattern.Id ?? 0) != 0)
            {
                patEntity =  Context.Patterns.Single(x => x.Id == pattern.Id.Value);
                if (patEntity.CategoryId != pattern.CategoryId)
                {
                    oldCategoryId = patEntity.CategoryId;
                }

                patEntity.CategoryId = pattern.CategoryId;
                patEntity.Phrase = pattern.Phrase;
                patEntity.Context = pattern.Context;
                patEntity.OnlyContext = pattern.OnlyContext;
                patEntity.WordCount = pattern.Words.Count();
            }
            else
            {
                patEntity = new Pattern()
                {
                    CategoryId = pattern.CategoryId,
                    Phrase = pattern.Phrase,
                    Context = pattern.Context,
                    OnlyContext = pattern.OnlyContext,
                    WordCount = pattern.Words.Count()
                };
                 Context.Patterns.Add(patEntity);
            }
             Context.SaveChanges();

            pattern.Id = patEntity.Id;
            StoreWords(pattern);

            var cat =  Context.Categorys.Single(x => x.Id == pattern.CategoryId);
            cat.IsChanged = true;
            cat.ChangedBy = _user.SigmaLogin;
            cat.ChangedOn = _dateTimeService.SessionUtcNow.ToLocalTime();
            if (oldCategoryId != 0)
            {
                var catOld =  Context.Categorys.Single(x => x.Id == oldCategoryId);
                catOld.IsChanged = true;
                catOld.ChangedBy = _user.SigmaLogin;
                catOld.ChangedOn = _dateTimeService.SessionUtcNow.ToLocalTime();
            }

             Context.SaveChanges();

        }

        private  void StoreWords(PatternDto pattern)
        {
            var pws = Context.PatternWordRels.Where(x => x.PatternId == pattern.Id.Value).ToList();
            Context.PatternWordRels.RemoveRange(pws);
            Context.SaveChanges();

            if (pattern.Words != null)
            {
                foreach (var w in pattern.Words)
                {
                    var wordEntity =  Context.Words.SingleOrDefault(x =>
                        x.WordName == w.WordName && (
                            (w.WordTypeId.HasValue && x.WordTypeId.HasValue && x.WordTypeId.Value == w.WordTypeId.Value) ||
                            (!w.WordTypeId.HasValue && !x.WordTypeId.HasValue)));
                    if (wordEntity == null)
                    {
                        wordEntity = new Word() { WordName = w.WordName, WordTypeId = w.WordTypeId };
                         Context.Words.Add(wordEntity);
                         Context.SaveChanges();
                        if (w.WordForms != null)
                        {
                            foreach (var wf in w.WordForms)
                            {
                                 Context.WordForms.Add(new WordForm
                                {
                                    WordId = wordEntity.Id,
                                    Form = wf.Form.Replace("ё", "е")
                                });
                            }
                        }
                    }

                    var pw = new PatternWordRel { WordId = wordEntity.Id, PatternId = pattern.Id.Value };
                    Context.PatternWordRels.Add(pw);
                }
                Context.SaveChanges();
            }
        }

    }
}
