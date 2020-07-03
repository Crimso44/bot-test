using LingvoNET;
using OfficeOpenXml;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static LingvoNET.Analyser;

namespace SBoT.Code.Entity
{
    public class WordFormer : IWordFormer
    {
        private readonly ISboTRepository _sBoTRepository;
        private readonly IFileTransformer _fileTransformer;

        private readonly string noDelimeters = "[];',.{}<>:\"";

        public WordFormer(ISboTRepository sBoTRepository, IFileTransformer fileTransformer)
        {
            _sBoTRepository = sBoTRepository;
            _fileTransformer = fileTransformer;
        }


        public bool IsLetter(char c, bool isForTranslit)
        {
            var res = (c >= 'a' && c <= 'z') || (c >= 'а' && c <= 'я') || (c >= '0' && c <= '9') || c == '-' || c == '_' || c == 'ё' || c == '*';
            if (!res && isForTranslit)
                res = noDelimeters.Contains(c);
            return res;
        }

        public WordListDto GetWordsFromPhrase(string phrase, bool isUnYo, bool isDistinct, bool isForTranslit)
        {
            var isDizzy = false; 
            var res = new List<string>();
            var s = "";
            phrase = phrase.ToLower();
            if (isUnYo) phrase = phrase.Replace("ё", "е");
            foreach (var c in phrase)
            {
                if (IsLetter(c, isForTranslit))
                {
                    s += c;
                }
                else
                {
                    if (!string.IsNullOrEmpty(s)) res.Add(s);
                    s = "";
                }
            }
            if (!string.IsNullOrEmpty(s)) res.Add(s);

            if (isForTranslit)
            {
                var res0 = new List<string>();
                foreach (var w in res)
                {
                    if (noDelimeters.Any(x => w.Contains(x)))
                    {
                        if (!w.Any(c => (c >= 'а' && c <= 'я') || c == 'ё'))
                        {
                            res0.Add(w);
                            isDizzy = true;
                        }
                        res0.AddRange(GetWordsFromPhrase(w, false, false, false).Words);
                    }
                    else
                    {
                        res0.Add(w);
                    }
                }
                res = res0;
            }

            if (isDistinct)
                res = res.Distinct().ToList();

            return new WordListDto() {Words = res, IsDizzy = isDizzy};
        }



        public FileDto GetReport(DateTime from, DateTime to, string rootPath)
        {
            var data = _sBoTRepository.GetReportData(from, to);
            var dataStat = _sBoTRepository.GetReportStatData(from, to);
            var bytes = _fileTransformer.MakeReport(data, dataStat, rootPath);
            return new FileDto { Body = bytes, Name = "Report.xlsx" };
        }




        ///<summary>
        /// Обработка заданных обязательных слов ( (... ...) и &lt;... ...&gt; в паттернах )
        ///</summary>
        public List<ResponseDto> CheckSequences(string question, List<string> words, List<ResponseDto> pts)
        {
            var res = new List<ResponseDto>();
            List<string> allWords = null;

            if (pts.Any())
            {

                var regSeq = new Regex("\\((.*?)\\)");
                var regOrd = new Regex("<(.*?)>");
                foreach (var pt in pts)
                {
                    if (res.Any() && res[0].ContextRate > pt.ContextRate) break;
                    if (res.Any(x => x.CategoryId == pt.CategoryId)) continue;

                    // слова в скобках - все должны присутствовать в вопросе
                    var matches = regSeq.Matches(pt.PatternPhrase);
                    if (matches.Count > 0)
                    {
                        var wordsSpelled = new List<string>();
                        wordsSpelled.AddRange(words);
                        if (pt.Weights != null)
                        {
                            foreach (var wt in pt.Weights)
                            {
                                wordsSpelled.AddRange(wt.Words);
                            }
                        }

                        var pat = _sBoTRepository.GetPatternWordsById(pt.PatternId);
                        var isOk = true;
                        foreach (Match match in matches)
                        {
                            var matchWords = GetWordsFromPhrase(match.Value, true, true, false).Words;
                            foreach (var matchWord in matchWords)
                            {
                                var wrds = pat.Words.First(x => x.WordName.Replace("ё", "е") == matchWord).WordForms
                                    .Select(x => x.Form).ToList();
                                if (!wrds.Intersect(wordsSpelled).Any())
                                {
                                    isOk = false;
                                    break;
                                }
                            }

                            if (!isOk) break;
                        }
                        if (!isOk) continue;
                    }

                    // слова в угловых скобках - все должны присутствовать в вопросе, причем в указанном порядке
                    matches = regOrd.Matches(pt.PatternPhrase);
                    if (matches.Count > 0)
                    {
                        if (allWords == null)
                            allWords = GetWordsFromPhrase(question, true, false, false).Words;

                        var pat = _sBoTRepository.GetPatternWordsById(pt.PatternId);
                        var isOk = true;
                        foreach (Match match in matches)
                        {
                            var matchWords = GetWordsFromPhrase(match.Value, true, false, false).Words;
                            List<int> prevIndxs = null;
                            foreach (var matchWord in matchWords)
                            {
                                var wrds = pat
                                    .Words.FirstOrDefault(x => x.WordName.Replace("ё", "е") == matchWord)?
                                    .WordForms.Select(x => x.Form).ToList();
                                if (wrds == null)
                                {
                                    isOk = false;
                                    break;
                                }
                                var indxs = Enumerable.Range(0, allWords.Count).Where(i =>
                                {
                                    var ok = wrds.Contains(allWords[i]);
                                    if (!ok && pt.Weights != null)
                                    {
                                        foreach (var wt in pt.Weights.Where(w => w.Word == allWords[i]))
                                        {
                                            ok = wt.Words.Intersect(wrds).Any();
                                            if (ok) return true;
                                        }
                                    }
                                    return ok;
                                }).ToList();
                                prevIndxs = prevIndxs == null ? indxs : indxs.Where(x => prevIndxs.Contains(x - 1)).ToList();
                                if (!prevIndxs.Any())
                                {
                                    isOk = false;
                                    break;
                                }

                            }

                            if (!isOk) break;
                        }
                        if (!isOk) continue;
                    }
                    res.Add(pt);
                }
            }

            return res;
        }



    }
}
