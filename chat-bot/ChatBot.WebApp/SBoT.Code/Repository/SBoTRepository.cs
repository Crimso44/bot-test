using AutoMapper;
using SBoT.Code.Dto;
using SBoT.Code.Repository.Interfaces;
using SBoT.Domain.DataModel.SBoT;
using SBoT.Domain.DataModel.SBoT.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SBoT.Code.Classes;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Services.Abstractions;
using Config = SBoT.Code.Classes.Config;
using SBoT.Connect.Abstractions.Dto;

namespace SBoT.Code.Repository
{
    public class SBoTRepository : ISboTRepository
    {
        private readonly ISBoTDataModel _ctx;
        private readonly IUserInfoService _user;
        private readonly IOptions<Config> _config;
        private readonly ITimeMeasurer _timeMeasurer;
        private readonly IRosterService _rosterService;
        private readonly IInfoService _infoService;

        private List<string> _fixedWords;
        private Dictionary<string, string> _settings = new Dictionary<string, string>();
        private DateTime _settingsTime = DateTime.Now;

        public SBoTRepository(ISBoTDataModel ctx, IUserInfoService user, ITimeMeasurer timeMeasurer, IRosterService rosterService, IInfoService infoService,
            IOptions<Config> config)
        {
            _ctx = ctx;
            _user = user;
            _config = config;
            _timeMeasurer = timeMeasurer;
            _rosterService = rosterService;
            _infoService = infoService;
        }

        public string GetSettings(string name)
        {
            if (_settingsTime.AddSeconds(30) < DateTime.Now || !_settings.ContainsKey(name))
            {
                var setting = _ctx.Configs.FirstOrDefault(x => x.Name == name);
                if (setting != null)
                {
                    _settings[name] = setting.Value;
                    _settingsTime = DateTime.Now;
                }
            }

            return _settings.ContainsKey(name) ? _settings[name] : null;
        }


        public List<ResponseDto> FindResponse(List<string> words, string context)
        {
            var t = DateTime.Now;
            var res = (from wf in _ctx.WordForms
                       join w in _ctx.Words on wf.WordId equals w.Id
                       join pw in _ctx.PatternWordRels on w.Id equals pw.WordId
                       join p in _ctx.Patterns on pw.PatternId equals p.Id
                       join c in _ctx.Categories on p.CategoryId equals c.Id
                       where !(c.IsTest ?? false) && !(c.IsDisabled ?? false) && !c.Name.StartsWith("--") && 
                            words.Contains(wf.Form) && 
                            (!(p.OnlyContext ?? false) || p.Context == context)
                       select new { c, p, w, wf }).ToList();

            var tt = DateTime.Now;
            _timeMeasurer.AddTimer("FindResponse", tt - t);

            var wrdCnt = words.Count; // res.Select(x => x.wf.Form).Distinct().Count();
            var pts = res.GroupBy(x => new { x.p, x.c }).Select(g => new ResponseDto
            {
                CategoryId = g.Key.c.Id,
                CategoryOriginId = g.Key.c.OriginId,
                CategoryRequiredRoster = (g.Key.c.RequiredRoster ?? "").Trim(),
                Category = g.Key.c.Name,
                Response = g.Key.c.Response,
                SetContext = g.Key.c.SetContext,

                PatternId = g.Key.p.Id,
                PatternPhrase = g.Key.p.Phrase,
                Context = g.Key.p.Context,
                PatternCnt = g.Key.p.WordCount.Value,
                FoundCnt = g.Count(),
                Rate = ((decimal)(g.Count() * g.Count())) / wrdCnt / g.Key.p.WordCount.Value,
                ContextRate =
                    ((decimal)(g.Count() * g.Count())) / wrdCnt / g.Key.p.WordCount.Value +
                    ((decimal)(string.IsNullOrEmpty(context) || context != g.Key.p.Context ? 0.0 : 1.0)) +
                    ((decimal)(g.Key.p.Phrase == "*" ? -0.1 : 0)),

                IsDefault = g.Key.c.IsDefault ?? false
            })
            .OrderByDescending(x => x.ContextRate)
            .ThenBy(x => x.CategoryId)
            .ToList();

            return pts;
        }

        public List<ResponseDto> FindResponseByWeights(List<WeightDto> weights, int wordCount, string context)
        {
            var t = DateTime.Now;

            var patIds = weights.Select(x => x.Id).Distinct().ToList();

            var res = (from p in _ctx.Patterns 
                       join c in _ctx.Categories on p.CategoryId equals c.Id
                       where !(c.IsTest ?? false) && !(c.IsDisabled ?? false) && !c.Name.StartsWith("--") &&
                            patIds.Contains(p.Id) &&
                            (!(p.OnlyContext ?? false) || p.Context == context)
                       select new { c, p }).ToList();

            var tt = DateTime.Now;
            _timeMeasurer.AddTimer("FindResponseByWeights", tt - t);

            var pts = (
                from w in weights
                join r in res on w.Id equals r.p.Id
                where !(r.c.IsIneligible ?? false) || (w.Words.Count == 1 && w.Word.ToLower() == w.Words[0])
                group w by r into g
                select new ResponseDto
            {
                CategoryId = g.Key.c.Id,
                CategoryOriginId = g.Key.c.OriginId,
                CategoryRequiredRoster = (g.Key.c.RequiredRoster ?? "").Trim(),
                Category = g.Key.c.Name,
                Response = g.Key.c.Response,
                SetContext = g.Key.c.SetContext,

                PatternId = g.Key.p.Id,
                PatternPhrase = g.Key.p.Phrase,
                Context = g.Key.p.Context,
                PatternCnt = g.Key.p.WordCount.Value,
                FoundCnt = g.Count(),
                Rate = ((decimal)(g.Sum(x => x.Weight) * g.Sum(x => x.Weight))) / wordCount / g.Key.p.WordCount.Value,
                ContextRate =
                    ((decimal)(g.Sum(x => x.Weight) * g.Sum(x => x.Weight))) / wordCount / g.Key.p.WordCount.Value +
                    ((decimal)(string.IsNullOrEmpty(context) || context != g.Key.p.Context ? 0.0 : 1.0)) +
                    ((decimal)(g.Key.p.Phrase == "*" ? -0.1 : 0)),

                IsDefault = g.Key.c.IsDefault ?? false,
                Weights = g.ToList()
            })
            .OrderByDescending(x => x.ContextRate)
            .ThenBy(x => x.CategoryId)
            .ToList();

            return pts;
        }

        public ResponseDto GetCategoryByName(string name, bool isTest)
        {
            var resp = _ctx.Categories.SingleOrDefault(c => (c.IsTest ?? false) == isTest && c.Name == name);
            if (resp == null) return null;
            return new ResponseDto
            {
                CategoryId = resp.Id,
                CategoryOriginId = resp.OriginId,
                CategoryRequiredRoster = (resp.RequiredRoster ?? "").Trim(),
                Category = resp.Name,
                Response = resp.Response,
                SetContext = resp.SetContext,
                Rate = 1
            };
        }

        public List<ResponseDto> GetResponsesFromOriginIds(List<Guid> originIds)
        {
            var resp = _ctx.Categories.Where(c => !(c.IsTest ?? false) && !(c.IsDisabled ?? false) && c.OriginId.HasValue && originIds.Contains(c.OriginId.Value)).ToList();
            return resp.Select(x => new ResponseDto
            {
                CategoryId = x.Id,
                CategoryOriginId = x.OriginId,
                CategoryRequiredRoster = (x.RequiredRoster ?? "").Trim(),
                Category = x.Name,
                Response = x.Response,
                SetContext = x.SetContext,
                Rate = 1,
                IsMto = true
            }).ToList();
        }


        public ResponseDto DefaultResponse()
        {
            return GetResponse(Const.CategoriesReserved.Default);
        }


        public ResponseDto GetResponse(string category)
        {
            var resp = _ctx.Categories.SingleOrDefault(x => x.Name == category && !(x.IsTest ?? false));

            return new ResponseDto
            {
                Response = resp?.Response,
                Rate = 0
            };
        }


        public int SaveHistory(string source, string question, string origQuestion, string context, ResponseDto response, bool isButton, string type)
        {
            if (_config.Value.IsReadOnly) return -1;

            var user = _user.User();
            var hist = new History
            {
                Source = source,
                QuestionDate = DateTime.Now,
                SigmaLogin = user.SigmaLogin,
                UserName = string.IsNullOrEmpty(user.Name) ? user.SigmaLogin : user.Name,
                Question = question,
                OriginalQuestion = origQuestion,
                Answer = response.Category,
                AnswerText = response.Response,
                AnswerType = type,
                Rate = response.Rate,
                Context = context,
                SetContext = response.SetContext,
                IsButton = isButton,
                CategoryOriginId = response.CategoryOriginId
            };
            _ctx.History.Add(hist);
            _ctx.SaveChanges();
            return hist.Id;
        }

        public void UpdateHistory(int historyId, string question, string origQuestion, string context, ResponseDto response, bool isButton, string type, bool isMto, string mtoTresholds)
        {
            if (_config.Value.IsReadOnly) return;

            var user = _user.User();
            var hist = _ctx.History.Single(x => x.Id == historyId);

            hist.QuestionDate = DateTime.Now;
            hist.SigmaLogin = user.SigmaLogin;
            hist.UserName = string.IsNullOrEmpty(user.Name) ? user.SigmaLogin : user.Name;
            hist.Question = question;
            hist.OriginalQuestion = origQuestion;
            hist.Answer = response.Category;
            hist.AnswerText = response.Response;
            hist.Rate = response.Rate;
            hist.Context = context;
            hist.SetContext = response.SetContext;
            hist.IsButton = isButton;
            hist.AnswerType = type;
            hist.CategoryOriginId = response.CategoryOriginId;
            hist.IsMto = response.IsMto;
            hist.MtoThresholds = mtoTresholds;
            _ctx.SaveChanges();
        }

        public bool SetLike(int historyId, short value)
        {
            var hist = _ctx.History.SingleOrDefault(x => x.Id == historyId);
            if (hist == null) return false;
            hist.Like = value;
            _ctx.SaveChanges();
            return true;
        }

        public List<ReportDto> GetReportData(DateTime from, DateTime to)
        {
            var data = _ctx.GetReports(from, to).GetAwaiter().GetResult();
            var codes = new Dictionary<string, string>();
            foreach(var d in data)
            {
                if (!string.IsNullOrEmpty(d.SigmaLogin))
                {
                    if (!codes.ContainsKey(d.SigmaLogin))
                    {
                        var code = _infoService.DecodeValue("табельный номер", null, false, d.SigmaLogin);
                        codes[d.SigmaLogin] = code;
                    }
                    d.TabNo = codes[d.SigmaLogin];
                }
            }
            return data.Select(Mapper.Map<ReportDto>).ToList();
        }

        public List<string> GetTestSetData(string set)
        {
            var data = _ctx.TestQuestions.Where(x => x.SetName == set).Select(x => x.Question).ToList();
            return data;
        }

        public List<ReportStatDto> GetReportStatData(DateTime from, DateTime to)
        {
            var data = _ctx.GetReportStats(from, to).GetAwaiter().GetResult();

            return data.Select(Mapper.Map<ReportStatDto>).ToList();
        }



        public PatternDto GetPatternWordsById(int id)
        {
            var req = (
                from p in _ctx.Patterns
                join pw in _ctx.PatternWordRels on p.Id equals pw.PatternId
                join w in _ctx.Words on pw.WordId equals w.Id
                join wf in _ctx.WordForms on w.Id equals wf.WordId
                where p.Id == id
                orderby w.Id
                select new { p, w, wf }).ToList();

            var res = Mapper.Map<PatternDto>(req[0].p);
            res.Words = new List<WordDto>();

            WordDto word = null;
            foreach (var d in req)
            {
                if (word == null || word.Id != d.w.Id)
                {
                    word = Mapper.Map<WordDto>(d.w);
                    word.WordForms = new List<WordFormDto>();
                    res.Words.Add(word);
                }

                var wfm = Mapper.Map<WordFormDto>(d.wf);
                word.WordForms.Add(wfm);
            }

            return res;
        }

        public List<HistoryDto> GetHistoryFrame(string sigmaLogin, int beforeId, int size)
        {
            return _ctx.History.AsNoTracking()
                .Where(h => h.SigmaLogin == sigmaLogin
                            && h.Id < beforeId
                            && h.AnswerText != null)
                .OrderByDescending(h => h.Id)
                .Take(size)
                .ToList()
                .Select(Mapper.Map<HistoryDto>)
                .ToList();
        }



        public bool CheckHello(List<string> words)
        {
            var res = (
                from wf in _ctx.WordForms
                join w in _ctx.Words on wf.WordId equals w.Id
                join pw in _ctx.PatternWordRels on w.Id equals pw.WordId
                join p in _ctx.Patterns on pw.PatternId equals p.Id
                join c in _ctx.Categories on p.CategoryId equals c.Id
                where words.Contains(wf.Form) && !(c.IsTest ?? false) && !(c.IsDisabled ?? false) && c.Name == Const.CategoriesReserved.HelloPatterns
                select wf).Any();
            return res;
        }

        public List<string> GetFixedWords()
        {
            if (_fixedWords == null)
            {
                _fixedWords = (
                    from wf in _ctx.WordForms
                    join w in _ctx.Words on wf.WordId equals w.Id
                    join pw in _ctx.PatternWordRels on w.Id equals pw.WordId
                    join p in _ctx.Patterns on pw.PatternId equals p.Id
                    join c in _ctx.Categories on p.CategoryId equals c.Id
                    where !(c.IsTest ?? false) && !(c.IsDisabled ?? false) &&
                          c.Name == Const.CategoriesReserved.FixedWords
                    select wf.Form).ToList();
            }
            return _fixedWords;
        }


        public string FindUserData(string question, Dictionary<string, RosterDto> roster)
        {
            var result = question;
            var regUser = new Regex("\\[((.*?):)?(.*?)\\|.*?\\]"); // [src:code|name]
            var match = regUser.Match(result);
            while (match.Success)
            {
                var src = match.Groups[2].Value;
                if (string.IsNullOrWhiteSpace(src) || !_rosterService.Roster().ContainsKey(src)) src = "E";
                if (roster != null)
                {
                    var data = _rosterService.GetByCode(match.Groups[3].Value, src);
                    if (data != null)
                        roster[data.Source] = data;
                }

                result = regUser.Replace(result, _rosterService.Roster()[src].Keyword);
                match = regUser.Match(result);
            }

            return result;
        }


        public List<WordListOutDto> GetCategoriesWords()
        {
            var res = (
                from wf in _ctx.WordForms
                join w in _ctx.Words on wf.WordId equals w.Id
                join pw in _ctx.PatternWordRels on w.Id equals pw.WordId
                join p in _ctx.Patterns on pw.PatternId equals p.Id
                join c in _ctx.Categories on p.CategoryId equals c.Id
                where !(c.IsTest ?? false) && !(c.IsDisabled ?? false)
                group new {wf, p} by c into g
                select new {g.Key.Name, g.Key.OriginId, ps = g.ToList()}).ToList()
                .Select(x => new WordListOutDto
                {
                    Category = x.Name,
                    CategoryOriginId = x.OriginId,
                    Patterns = x.ps.GroupBy(g => g.p)
                        .Select(g => new PatternLiteDto() { 
                            Phrase = g.Key.Phrase,
                            Words = g.Select(wf => wf.wf.Form).Distinct().ToList()
                        }).ToList()
                }).ToList();
            return res;
        }


        public List<WordIndexDto> GetWordsForIndex()
        {
            var res = (
                from wf in _ctx.WordForms
                join w in _ctx.Words on wf.WordId equals w.Id
                join pw in _ctx.PatternWordRels on w.Id equals pw.WordId
                join p in _ctx.Patterns on pw.PatternId equals p.Id
                join c in _ctx.Categories on p.CategoryId equals c.Id
                where !(c.IsTest ?? false) && !(c.IsDisabled ?? false)
                group new {wf, p} by wf.Form
                into g
                select new {word = g.Key, pattern = g.Select(x => x.p.Id).Distinct().ToList()});
            return res.Select(x => new WordIndexDto()
            {
                Id = x.word,
                Pattern = x.pattern
            }).ToList();
        }


    }

    public class SBoTRepositoryTransient : SBoTRepository, ISboTRepositoryTransient
    {
        public SBoTRepositoryTransient(ISBoTDataModelTransient ctx, IUserInfoService user, 
            ITimeMeasurer timeMeasurer, IRosterService rosterService, IInfoService infoService, IOptions<Config> config)
            : base(ctx, user, timeMeasurer, rosterService, infoService, config)
        {
        }
    }
}