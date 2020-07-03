using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository.Interfaces;
using SBoT.Domain.Const;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using SBoT.Code.Services.Abstractions;
using ResponseDto = SBoT.Code.Dto.ResponseDto;

namespace SBoT.Code.Entity
{
    public class Chatter : IChatter
    {
        private const int LinkCountLimit = 7;

        private readonly ISboTRepository _sBoTRepository;
        private readonly IUsefulLinksRepository _usefulLinksRepository;
        private readonly IWordFormer _wordFormer;
        private readonly IUserInfoService _user;
        private readonly IRabbitWorker _rabbitWorker;
        private readonly IElasticWorker _elasticWorker;
        private readonly IFileTransformer _fileTransformer;
        private readonly ITimeMeasurer _timeMeasurer;
        private readonly IOptions<Config> _config;
        private readonly IOptions<Urls> _urls;
        private readonly IWebRequestProcess _request;
        private readonly IRosterService _rosterService;
        private readonly IInfoService _infoService;

        private readonly string translitFrom = "qwertyuiop[]asdfghjkl;'zxcvbnm,.{}<>:\"йцукенгшщзхъфывапролджэячсмитьбюхъбюжэ";
        private readonly string translitTo =   "йцукенгшщзхъфывапролджэячсмитьбюхъбюжэqwertyuiop[]asdfghjkl;'zxcvbnm,.{}<>:\"";

        private readonly Guid _id = Guid.NewGuid();

        public Chatter(ISboTRepository sBoTRepository, IWordFormer wordFormer, IUserInfoService user, 
            IUsefulLinksRepository usefulLinksRepository, ITimeMeasurer timeMeasurer, IElasticWorker elasticWorker,
            IRabbitWorker rabbitWorker, IFileTransformer fileTransformer, 
            IOptions<Config> config, IOptions<Urls> urls, IWebRequestProcess request, IRosterService rosterService, IInfoService infoService)
        {
            _sBoTRepository = sBoTRepository;
            _wordFormer = wordFormer;
            _user = user;
            _usefulLinksRepository = usefulLinksRepository;
            _rabbitWorker = rabbitWorker;
            _elasticWorker = elasticWorker;
            _fileTransformer = fileTransformer;
            _timeMeasurer = timeMeasurer;
            _config = config;
            _urls = urls;
            _request = request;
            _rosterService = rosterService;
            _infoService = infoService;
        }

        public Guid Id()
        {
            return _id;
        }
        private string Transliterate(string question)
        {
            var res = "";
            var questLower = question.ToLower();
            var inKeyword = false;
            for (var i = 0; i < questLower.Length; i++)
            {
                var c = questLower[i];
                if (inKeyword)
                {
                    res += c;
                    inKeyword = _wordFormer.IsLetter(c, false);
                }
                else
                {
                    if (c == '_')
                    {
                        var tail = questLower.Substring(i);
                        inKeyword = _rosterService.Roster().Values.Any(x => tail.StartsWith(x.Keyword));
                    }
                    var j = translitFrom.IndexOf(c);
                    if (!inKeyword && j >= 0)
                        res += translitTo[j];
                    else
                        res += c;
                }
            }

            return res;
        }


        private string ChangeQuestion(string question, List<ResponseDto> resps)
        {
            var qWords = _wordFormer.GetWordsFromPhrase(question, true, false, false);

            foreach (var r in resps)
            {
                foreach (var w in r.Weights)
                {
                    if (w.Words.Any() && w.Word != w.Words[0])
                    {
                        for (var i = 0; i < qWords.Words.Count; i++)
                        {
                            if (qWords.Words[i] == w.Word) qWords.Words[i] = w.Words[0];
                        }
                    }
                }
            }

            return string.Join(" ", qWords.Words);
        }

        public ResponseListDto FindResponseWithSequences(int histId, string question, string context, bool isSilent, bool isMto)
        {
            var isTranslitChecked = false;
            var isSimplyAsked = false;
            var res = new ResponseListDto() {NewQuestion = question, IsDizzy = false, IsMtoAnswer = false, Responses = new List<ResponseDto>()};

            //var rabbitId = Guid.NewGuid();
            var words = _wordFormer.GetWordsFromPhrase(question, true, true, false).Words;

            if (_config.Value.IsUseMto && isMto && _sBoTRepository.GetSettings(Const.Settings.UseModel) != "False")
            {
                var useMLThreshold = _sBoTRepository.GetSettings(Const.Settings.UseMLThreshold) == "True";
                var useMLMultiAnswer = _sBoTRepository.GetSettings(Const.Settings.UseMLMultiAnswer) == "True";
                float mlThreshold = 0;
                float.TryParse(_sBoTRepository.GetSettings(Const.Settings.MLThreshold), out mlThreshold);
                float mlMultiThreshold = 0;
                float.TryParse(_sBoTRepository.GetSettings(Const.Settings.MLMultiThreshold), out mlMultiThreshold);
                var mtoId = Guid.NewGuid();
                if (_rabbitWorker.SendMtoQuestion(mtoId, new List<string>() {question}))
                {
                    var mtoAnswers = _rabbitWorker.ReceiveMtoAnswers(mtoId);
                    if (mtoAnswers != null && mtoAnswers.Any())
                    {
                        res.IsMtoAnswer = false;
                        if (mtoAnswers[0].answer.Any())
                        {
                            var answers = mtoAnswers[0].answer.OrderByDescending(x => x.rate).ToList();
                            res.MtoThresholds = string.Join("; ",answers.Select(x => $"{x.rate:f2}").ToList());

                            if (useMLMultiAnswer)
                            {
                                answers = answers.Where(x => x.rate >= mlMultiThreshold).ToList();
                            }
                            else
                            {
                                answers = new List<ProbabilityDto>() { answers.First() };
                                if (useMLThreshold) answers = answers.Where(x => x.rate >= mlThreshold).ToList();
                            }

                            var answerGuids = new List<Guid>();
                            foreach (var answer in answers)
                            {
                                if (Guid.TryParse(answer.id, out var answerGuid))
                                {
                                    answerGuids.Add(answerGuid);
                                }
                            }

                            isTranslitChecked = true;
                            res.Responses = _sBoTRepository.GetResponsesFromOriginIds(answerGuids).ToList();
                            res.IsMtoAnswer = true;
                        }
                        res.ModelResponse = mtoAnswers[0].message;
                    }
                };
            }

            if (!res.Responses.Any())
            {
                if (_config.Value.IsElastic)
                {
                    var respEl = _elasticWorker.FindResponse(words, context);
                    respEl = _wordFormer.CheckSequences(question, words, respEl);
                    if (respEl.Any())
                    {
                        res.Responses = respEl;
                        res.NewQuestion = ChangeQuestion(question, respEl);
                    }
                }
                else
                {
                    res.Responses = _sBoTRepository.FindResponse(words, context);
                    res.Responses = _wordFormer.CheckSequences(question, words, res.Responses);
                    isSimplyAsked = true;
                }
            }

            if (!isTranslitChecked)
            {
                var questTrans = Transliterate(question);
                words = _wordFormer.GetWordsFromPhrase(questTrans, true, true, false).Words;
                List<ResponseDto> respTrans;
                if (_config.Value.IsElastic)
                {
                    respTrans = _elasticWorker.FindResponse(words, context);
                }
                else
                {
                    respTrans = _sBoTRepository.FindResponse(words, context);
                }
                respTrans = _wordFormer.CheckSequences(questTrans, words, respTrans);

                if (respTrans.Any() && (!res.Responses.Any() || res.Responses[0].ContextRate < respTrans[0].ContextRate))
                {
                    res.NewQuestion = questTrans;
                    res.Responses = respTrans;
                    if (_config.Value.IsElastic)
                    {
                        res.NewQuestion = ChangeQuestion(questTrans, respTrans);
                    }
                }
            }

            if (!res.Responses.Any() && !isSimplyAsked)
            {
                res.Responses = _sBoTRepository.FindResponse(words, context);
                res.Responses = _wordFormer.CheckSequences(question, words, res.Responses);
            }

            if (!isSilent && res.Responses.Any()) res.Links = _usefulLinksRepository.SearchLinks(res.NewQuestion);

            return res;
        }

        private bool ComparePhrases(string strOld, string strNew)
        {
            var xstrOld = string.Join(" ", _wordFormer.GetWordsFromPhrase(strOld, true, true, false).Words);
            var xstrNew = string.Join(" ", _wordFormer.GetWordsFromPhrase(strNew, true, true, false).Words);
            return xstrOld == xstrNew;
        }

        ///<summary>
        /// Отпиливаем приветствия в начале вопроса (Привет, Ева! Скажи ...)
        ///</summary>
        private ResponseListDto TryToDeleteHello(int histId, string question, string context, bool isSilent, bool isMto)
        {
            var res = new ResponseListDto() { NewQuestion = question, IsTransliterated = false, IsDizzy = false };
            question = question.Trim();
            var split = question.Split('.', '!');
            if (split.Length < 2)
            {
                split = question.Split(',');
            }

            if (split.Length > 1)
            {
                var words = _wordFormer.GetWordsFromPhrase(split[0], true, true, false).Words;
                if (_sBoTRepository.CheckHello(words))
                {
                    split[0] = "";
                    res.NewQuestion = string.Join(" ", split).Trim();
                    var newResps = FindResponseWithSequences(histId, res.NewQuestion, context, isSilent, isMto);
                    res.Responses = newResps.Responses;
                    if (!ComparePhrases(res.NewQuestion, newResps.NewQuestion))
                    {
                        res.NewQuestion = newResps.NewQuestion;
                        res.IsTransliterated = true;
                        res.IsDizzy = newResps.IsDizzy;
                        res.Links = newResps.Links;
                    }
                    res.IsMtoAnswer = newResps.IsMtoAnswer;
                    res.ModelResponse = newResps.ModelResponse;
                }
            }

            if (res.Responses == null) res.Responses = new List<ResponseDto>();
            return res;
        }

        public AnswerDto AskBot(string source, string question, string context)
        {
            return AskBotEx(source, question, context, false, true, false);
        }

        public AnswerDto AskBotByMail(string source, string question, string mail)
        {
            _user.SetCurrentUserByMail(mail);
            return AskBotEx(source, question, null, false, true, true);
        }

        public AnswerDto AskBotEx(string source, string question, string context, bool isSilent, bool isMto, bool isCheckSbt)
        {
            ResponseDto response;

            var histId = 0;
            if (!isSilent)
                histId = _sBoTRepository.SaveHistory(source, question, question, context, new ResponseDto(), false, null);
            var answerType = "";

            var questOrig = question;

            if (question?.ToLower().Contains("обратная связь") ?? false) question = "обратная связь";

            var roster = new Dictionary<string, RosterDto>();
            question = _sBoTRepository.FindUserData(question, roster);

            if (isCheckSbt && _config.Value.IsCheckSbt)
            {
                var check = _infoService.DecodeValue("сбт", roster, false, null);
                if (string.IsNullOrEmpty(check))
                {
                    response = _sBoTRepository.GetCategoryByName(Const.CategoriesReserved.NoSbt, false);
                    _sBoTRepository.UpdateHistory(histId, question, questOrig, context, response, false, "NoSbt", false, null);
                    var defRes = new AnswerDto
                    {
                        Id = histId,
                        Rate = response.Rate,
                        Title = response.Response,
                        Context = context,
                        IsLikeable = false,
                        QuestionChanged = question,
                        IsMtoAnswer = false,
                        ModelResponse = ""
                    };
                    return defRes;
                }
            }


            var resps = TryToDeleteHello(histId, question, context, isSilent, isMto);
            if (resps.Responses.Any())
            {
                question = resps.NewQuestion;
            }
            else
            {
                resps = FindResponseWithSequences(histId, question, context, isSilent, isMto);
                if (resps.Responses.Any() && !ComparePhrases(question, resps.NewQuestion))
                {
                    question = resps.NewQuestion;
                    resps.IsTransliterated = true;
                }
            }

            if (!resps.Responses.Any())
            {
                var def = _sBoTRepository.DefaultResponse();

                if (!isSilent)
                {
                    if (resps.Links != null && resps.Links.Any())
                    {
                        var linkTxt = _usefulLinksRepository.FormatLinks(resps.Links, LinkCountLimit);
                        var respLinks = _sBoTRepository.GetResponse(Const.CategoriesReserved.UsefulLinksOnly);
                        if (string.IsNullOrEmpty(respLinks.Response))
                            def.Response = "Я нашла такие ссылки:<br/>" + linkTxt;
                        else
                            def.Response = string.Format(respLinks.Response, linkTxt);
                        def.Rate = (decimal)0.5;
                        def.ContextRate = (decimal)0.5;
                    }

                    _sBoTRepository.UpdateHistory(histId, question, questOrig, context, def, false, "NoAnswer", false, null);
                }

                var defRes = new AnswerDto
                {
                    Id = histId, Rate = def.Rate, Title = def.Response, Context = context, IsLikeable = false, QuestionChanged = resps.NewQuestion,
                    IsMtoAnswer = resps.IsMtoAnswer, ModelResponse = resps.ModelResponse
                };
                return defRes;
            }

            if (resps.Responses.Count == 1)
            {
                response = resps.Responses[0];
                if (!string.IsNullOrWhiteSpace(response.CategoryRequiredRoster) && !roster.ContainsKey(response.CategoryRequiredRoster))
                {
                    response.SetContext = UnfindUserData(response.Category, roster);
                    response.Response = _rosterService.Roster()[response.CategoryRequiredRoster].Text +
                                        $"<xrst type='{response.CategoryRequiredRoster}'>{response.Category}</xrst>";
                }
            }
            else
            {
                answerType = "ManyAnswers";

                var resp = AppSettingsConst.SpecifyMessage + ":<br/>";
                foreach(var r in resps.Responses)
                {
                    resp += UnfindUserData($"<xbtn category='{r.Category}", roster);
                    resp += $"'>{r.Category}</xbtn>";
                }
                resp += "<br/><br/><xlnk category='another' context='another'>Ни один из вариантов не подходит?</xlnk>";

                response = new ResponseDto
                {
                    IsDefault = true,
                    Rate = resps.Responses[0].ContextRate,
                    Response = resp,
                    Context = context,
                    IsMto = resps.Responses[0].IsMto
                };
            }

            if (!isSilent) {

                if (!response.Response.Contains("<xrst"))
                {
                    if (resps.IsTransliterated)
                    {
                        if (resps.IsDizzy)
                        {
                            response.Response = "Похоже, Вы забыли переключить раскладку клавиатуры...<br/>" +
                                                response.Response;
                        }
                        else
                        {
                            var quest = question;
                            foreach (var kw in _rosterService.Roster().Keys)
                                quest = quest.Replace(_rosterService.Roster()[kw].Keyword, "");
                            var respChanged = _sBoTRepository.GetResponse(Const.CategoriesReserved.Changed);
                            if (string.IsNullOrEmpty(respChanged.Response))
                                response.Response = "Вы имели в виду <span class='color-red'>" + quest.Trim() +
                                                    "</span> ?<br/>" + response.Response;
                            else
                                response.Response =
                                    string.Format(respChanged.Response,
                                        "<span class='color-red'>" + quest.Trim() + "</span>") + "<br/>" +
                                    response.Response;
                        }
                    }

                    if (resps.Links != null && resps.Links.Any())
                    {
                        var linkTxt = _usefulLinksRepository.FormatLinks(resps.Links, LinkCountLimit);
                        response.Response += "<br/>";
                        var respLinks = _sBoTRepository.GetResponse(Const.CategoriesReserved.UsefulLinks);
                        if (string.IsNullOrEmpty(respLinks.Response))
                            response.Response += "А еще я нашла такие ссылки:<br/>" + linkTxt;
                        else
                            response.Response += string.Format(respLinks.Response, linkTxt);
                    }
                }

                response.Response = FillResponseWithData(response.Response, roster);
                _sBoTRepository.UpdateHistory(histId, question, questOrig, response.ContextRate > 1 ? context : "", response, false, answerType, response.IsMto, resps.MtoThresholds);
            }

            var res = new AnswerDto {
                Id = histId,
                Rate = response.Rate,
                Title = response.Response,
                Context = string.IsNullOrEmpty(response.SetContext) ? response.Context : response.SetContext,
                IsLikeable = !response.IsDefault,
                QuestionChanged = resps.NewQuestion,
                IsMto = response.IsMto,
                IsMtoAnswer = resps.IsMtoAnswer,
                OriginalCategorys =
                    resps.Responses.Where(x => !isMto || x.IsMto).Select(x => new Pair<Guid>() { Id = x.CategoryOriginId.Value, Title = x.Category }).ToList(),
                ModelResponse = resps.ModelResponse
            };

            if (res.Title.Contains("<xrst")) res.IsLikeable = false;

            return res;
        }

        private string UnfindUserData(string question, Dictionary<string, RosterDto> roster)
        {
            var res = question;
            foreach (var key in roster.Keys)
            {
                var rst = roster[key];
                res += $" [{rst.Source}:{rst.Code}|{rst.Name}]";
            }
            return res;
        }

        public AnswerDto AskBotByButtonMail(string source, string mail, string question, string category)
        {
            _user.SetCurrentUserByMail(mail);
            return AskBotByButton(source, question, null, category, true);
        }


        public AnswerDto AskBotByButton(string source, string question, string context, string category, bool isCheckSbt)
        {
            ResponseDto response;
            var roster = new Dictionary<string, RosterDto>();

            if (isCheckSbt && _config.Value.IsCheckSbt)
            {
                var check = _infoService.DecodeValue("сбт", roster, false, null);
                if (string.IsNullOrEmpty(check))
                {
                    response = _sBoTRepository.GetCategoryByName(Const.CategoriesReserved.NoSbt, false);
                    var hId = _sBoTRepository.SaveHistory(source, question, $"({category})", context, response, true, null);
                    var defRes = new AnswerDto
                    {
                        Id = hId,
                        Rate = response.Rate,
                        Title = response.Response,
                        Context = context,
                        IsLikeable = false,
                        QuestionChanged = question,
                        IsMtoAnswer = false,
                        ModelResponse = ""
                    };
                    return defRes;
                }
            }

            category = _sBoTRepository.FindUserData(category, roster);
            foreach(var key in _rosterService.Roster().Keys)
                category = category.Replace(_rosterService.Roster()[key].Keyword, "").Trim();

            response = _sBoTRepository.GetCategoryByName(category, false);
            if (!string.IsNullOrWhiteSpace(response.CategoryRequiredRoster) && !roster.ContainsKey(response.CategoryRequiredRoster))
            {
                response.SetContext = UnfindUserData(response.Category, roster);
                response.Response = _rosterService.Roster()[response.CategoryRequiredRoster].Text +
                                    $"<xrst type='{response.CategoryRequiredRoster}'>{response.Category}</xrst>";
            }
            else
            {
                response.Response = FillResponseWithData(response.Response, roster);
            }

            var histId = 0;
            if (category != Const.CategoriesReserved.HelloMessage)
                histId = _sBoTRepository.SaveHistory(source, question, $"({category})", context, response, true, null);
            var res = new AnswerDto {
                Id = histId,
                Rate = response.Rate,
                Title = response.Response,
                Context = string.IsNullOrEmpty(response.SetContext) ? context : response.SetContext,
                IsLikeable = !response.IsDefault && response.Category != "dislike"
            };
            if (res.Title.Contains("<xrst") || context == "another") res.IsLikeable = false;

            return res;
        }


        private string FillResponseWithData(string answer, Dictionary<string, RosterDto> roster)
        {
            var result = answer;

            var userData = UnfindUserData("", roster);

            var regXIf = new Regex("<xif\\sshow=['\"](.*?)['\"]>([\\s\\S]*?)<\\/xif>");
            var match = regXIf.Match(result);
            while (match.Success)
            {
                var check = _infoService.DecodeValue(match.Groups[1].Value, roster, false, null);
                result = result.Replace(match.Value, string.IsNullOrEmpty(check) ? "" : match.Groups[2].Value);
                match = match.NextMatch();
            }
            regXIf = new Regex("<xif\\shide=['\"](.*?)['\"]>([\\s\\S]*?)<\\/xif>");
            match = regXIf.Match(result);
            while (match.Success)
            {
                var check = _infoService.DecodeValue(match.Groups[1].Value, roster, false, null);
                result = result.Replace(match.Value, string.IsNullOrEmpty(check) ? match.Groups[2].Value : "");
                match = match.NextMatch();
            }

            var regXBtn = new Regex("<xbtn\\scategory=['\"](.*?)['\"]>");
            match = regXBtn.Match(result);
            while (match.Success)
            {
                var matchStr = match.Groups[0].Value;
                matchStr = matchStr.Substring(0, matchStr.Length - 2) + userData + matchStr.Substring(matchStr.Length - 2);
                result = result.Replace(match.Value, matchStr);

                match = match.NextMatch();
            }

            var regOrg = new Regex("<org>(.*?)</org>");
            match = regOrg.Match(result);
            while (match.Success)
            {
                var data = match.Groups[1].Value;
                var replace = _infoService.DecodeValue(data, roster, true, null);
                data = replace ?? data;
                result = result.Replace(match.Value, "<b>" + data + "</b>");
                match = regOrg.Match(result);
            }

            var regXOrg = new Regex("<xorg.*?>(.*?)</xorg>");
            var regXCat = new Regex("\\scategory=['\"](.*?)['\"]");
            var regXCat0 = new Regex("\\scategory0=['\"](.*?)['\"]");
            var regXCat0Text = new Regex("\\scategory0-text=['\"](.*?)['\"]");
            var regXNo = new Regex("\\snot-found=['\"](.*?)['\"]");
            match = regXOrg.Match(result);
            while (match.Success)
            {
                var text = match.Groups[1].Value;
                var matchCat = regXCat.Match(match.Value);
                if (matchCat.Success)
                {
                    var category = matchCat.Groups[1].Value;
                    var matchNo = regXNo.Match(match.Value);
                    var matchCat0 = regXCat0.Match(match.Value);
                    var matchCat0Text = regXCat0Text.Match(match.Value);

                    string replace = null;
                    if (matchCat0.Success)
                    {
                        var category0 = matchCat0.Groups[1].Value;
                        replace = _infoService.DecodeValue(category0, roster, !matchNo.Success, null);

                        if (replace != null && matchCat0Text.Success)
                        {
                            text = matchCat0Text.Groups[1].Value;
                        }
                    }

                    if (replace == null)
                    {
                        replace = _infoService.DecodeValue(category, roster, !matchNo.Success, null);
                    }

                    if (string.IsNullOrEmpty(replace) && matchNo.Success)
                    {
                        text = matchNo.Groups[1].Value;
                    }
                    else if (text.Contains("{0}"))
                    {
                        text = text.Replace("{0}", replace);
                    }
                    else
                    {
                        text = replace;
                    }

                }

                result = result.Replace(match.Value, text);
                match = regXOrg.Match(result);
            }

            return result;
        }


        public bool SetLike(Pair<int> like)
        {
            if (short.TryParse(like.Title, out short val))
            {
                return _sBoTRepository.SetLike(like.Id, val);
            }
            return false;
        }


        public string GetUserFirstName()
        {
            var firstName = _infoService.DecodeValue("имя", null, false, null);
            return firstName;
        }


        public FileDto GetReportMto(DateTime from, DateTime to, string rootPath)
        {
            _timeMeasurer.SetTimer("FindResponse");

            var data = _sBoTRepository.GetReportData(from, to);
            var res01 = new List<ReportSpellDto>();
            var cnt = 0;
            var cntAll = 0;
            var cnt01 = 0;
            var time0 = TimeSpan.Zero;
            var time1 = TimeSpan.Zero;
            foreach (var d in data)
            {
                cnt++;
                if (!d.OriginalQuestion.StartsWith("("))
                {
                    cntAll++;
                    var now = DateTime.Now;
                    var answer0 = AskBotEx("Rep", d.OriginalQuestion, d.ContextIn, true, false, false);
                    time0 += (DateTime.Now - now);
                    now = DateTime.Now;
                    var answer1 = AskBotEx("Rep", d.OriginalQuestion, d.ContextIn, true, true, false);
                    time1 += (DateTime.Now - now);
                    if (answer0.Title != answer1.Title)
                    {
                        cnt01++;
                        res01.Add(new ReportSpellDto()
                        {
                            OriginalQuestion = d.OriginalQuestion,
                            AnswerFirst = answer0.Title,
                            AnswerSecond = answer1.Title,
                            QuestionFirst = answer0.QuestionChanged,
                            QuestionSecond = answer1.QuestionChanged,
                            IsMto = answer1.IsMto
                        });
                    }
                    Debug.Write($"{cnt} from {data.Count} {cnt01} {time0}-{time1} ... {d.OriginalQuestion}\n");
                }
            }


            var bytes = _fileTransformer.MakeReportMto(res01, cntAll, time0, time1, rootPath);
            return new FileDto { Body = bytes, Name = "ReportMto.xlsx" };
        }


        public FileDto GetReportMtoCompare(string set, string rootPath)
        {
            _timeMeasurer.SetTimer("FindResponse");

            var data = _sBoTRepository.GetTestSetData(set);
            var res01 = new List<ReportMtoDto>();
            var cnt = 0;
            var cnt01 = 0;
            var time0 = TimeSpan.Zero;
            var time1 = TimeSpan.Zero;
            foreach (var d in data)
            {
                cnt++;
                var now = DateTime.Now;
                var answer0 = AskBotEx("Rep", d, "", true, false, false);
                time0 += (DateTime.Now - now);
                now = DateTime.Now;
                var answer1 = AskBotEx("Rep", d, "", true, true, false);
                time1 += (DateTime.Now - now);

                if (answer0.Title != answer1.Title) cnt01++;

                res01.Add(new ReportMtoDto()
                {
                    Question = d,
                    OriginalCategorysElastic = answer0.OriginalCategorys,
                    IsChanged = answer0.Title != answer1.Title,
                    IsMtoAnswer = answer1.IsMtoAnswer,
                    OriginalCategorys = answer1.OriginalCategorys,
                    ModelResponse = answer1.ModelResponse
                });
                Debug.Write($"{cnt} from {data.Count} {cnt01} {time0}-{time1} ... {d}\n");
            }


            var bytes = _fileTransformer.MakeReportMtoCompare(res01, cnt01, time0, time1, rootPath);
            return new FileDto {Body = bytes, Name = "ReportMtoCompare.xlsx"};
        }


        public List<ReportDto> GetReportData(DateTime from, DateTime to)
        {
            return _sBoTRepository.GetReportData(from, to).ToList();
        }


        public FileDto GetReportMtoAnswers(DateTime from, DateTime to, string rootPath)
        {
            var data = _sBoTRepository.GetReportData(from, to);

            var res01 = new List<ReportMtoDto>();
            var cnt = 0;
            foreach (var d in data)
            {
                cnt++;
                if (!d.OriginalQuestion.StartsWith("("))
                {
                    var answer1 = AskBotEx("Rep", d.OriginalQuestion, d.ContextIn, true, true, false);
                    res01.Add(new ReportMtoDto()
                    {
                        Question = d.OriginalQuestion,
                        IsMtoAnswer = answer1.IsMtoAnswer,
                        OriginalCategorys = answer1.OriginalCategorys,
                        ModelResponse = answer1.ModelResponse
                    });
                    Debug.Write($"{cnt} from {data.Count} ... {d.OriginalQuestion}\n");
                }
            }


            var bytes = _fileTransformer.MakeReportMtoAnswers(res01, rootPath);
            return new FileDto { Body = bytes, Name = "ReportMtoAnswers.xlsx" };
        }


    }

    public class ChatterTransient : Chatter, IChatterTransient
    {
        public ChatterTransient(ISboTRepository sBoTRepository, IWordFormer wordFormer, IUserInfoService user,
            IUsefulLinksRepository usefulLinksRepository, ITimeMeasurer timeMeasurer, IElasticWorker elasticWorker,
            IRabbitWorker rabbitWorker, IFileTransformer fileTransformer,
            IOptions<Config> config, IOptions<Urls> urls, IWebRequestProcess request, IRosterService rosterService, IInfoService infoService) : 
            base (sBoTRepository, wordFormer, user, usefulLinksRepository, timeMeasurer, elasticWorker, 
                rabbitWorker, fileTransformer, config, urls, request, rosterService, infoService)
        {
        }
    }

}