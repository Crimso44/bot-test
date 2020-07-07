using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LingvoNET;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Const.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommonServices.Services
{
    public class WordService : IWordService
    {
        private readonly string noDelimeters = "[];',.{}<>:\"";

        public void FillWordForms(WordDto word, List<string> errors)
        {
            var wfs = GetWordFormList(word, errors);
            word.WordForms = wfs.Select(x => new WordFormDto { Form = x }).ToList();
        }


        private List<string> GetWordFormList(WordDto w, List<string> errors)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var wfs = new List<string>() { w.WordName };
            if (w.WordTypeId.HasValue)
            {
                var v = "";
                switch ((ChatBotConst.WordType)w.WordTypeId.Value)
                {
                    case ChatBotConst.WordType.Noun:
                        var nouns = Nouns.FindAll(w.WordName);
                        if (!nouns.Any())
                        {
                            var noun = Nouns.FindSimilar(w.WordName);
                            if (noun != null) nouns = new List<Noun> { noun };
                        }
                        if (nouns.Any())
                        {
                            foreach (var noun in nouns)
                            {
                                v = noun[Case.Genitive]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Dative]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Accusative]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Instrumental]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Locative]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Nominative, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Genitive, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Dative, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Accusative, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Instrumental, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = noun[Case.Locative, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                            }
                        }
                        else
                        {
                            errors?.Add($"Похоже, что '{w.WordName}' не существительное");
                        }
                        break;
                    case ChatBotConst.WordType.Verb:
                        var verbs = Verbs.FindAll(w.WordName);
                        if (!verbs.Any())
                        {
                            var verb = Verbs.FindSimilar(w.WordName);
                            if (verb != null) verbs = new List<Verb> { verb };
                        }
                        if (verbs.Any())
                        {
                            foreach (var verb in verbs)
                            {
                                v = verb[Voice.Active, Person.First, Number.Singular]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Passive, Person.First, Number.Singular]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Active, Person.Second, Number.Singular]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Passive, Person.Second, Number.Singular]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Active, Person.Third, Number.Singular]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Passive, Person.Third, Number.Singular]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Active, Person.First, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Passive, Person.First, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Active, Person.Second, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Passive, Person.Second, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Active, Person.Third, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb[Voice.Passive, Person.Third, Number.Plural]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Passive, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Passive, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Passive, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Passive, Gender.P); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Active, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Active, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Active, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Past(Voice.Active, Gender.P); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                /* буду <что-то делать> - такие нам не надо
                                v = verb.Future(Voice.Passive, Person.First, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Passive, Person.First, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Passive, Person.Second, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Passive, Person.Second, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Passive, Person.Third, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Passive, Person.Third, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Active, Person.First, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Active, Person.First, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Active, Person.Second, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Active, Person.Second, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Active, Person.Third, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.Future(Voice.Active, Person.Third, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);*/
                                v = verb.FuturePerfect(Voice.Passive, Person.First, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Passive, Person.First, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Passive, Person.Second, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Passive, Person.Second, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Passive, Person.Third, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Passive, Person.Third, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Active, Person.First, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Active, Person.First, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Active, Person.Second, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Active, Person.Second, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Active, Person.Third, Number.Singular); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = verb.FuturePerfect(Voice.Active, Person.Third, Number.Plural); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                            }
                        }
                        else
                        {
                            errors?.Add($"Похоже, что '{w.WordName}' не глагол");
                        }
                        break;
                    case ChatBotConst.WordType.Adjective:
                        var adjs = Adjectives.FindAll(w.WordName);
                        if (!adjs.Any())
                        {
                            var adj = Adjectives.FindSimilar(w.WordName);
                            if (adj != null) adjs = new List<Adjective> { adj };
                        }
                        if (adjs.Any())
                        {
                            foreach (var adj in adjs)
                            {
                                v = adj[Case.Nominative, Gender.F]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Nominative, Gender.N]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Nominative, Gender.P]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Genitive, Gender.M]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Genitive, Gender.F]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Genitive, Gender.N]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Genitive, Gender.P]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Dative, Gender.M]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Dative, Gender.F]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Dative, Gender.N]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Dative, Gender.P]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Accusative, Gender.M]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Accusative, Gender.F]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Accusative, Gender.N]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Accusative, Gender.P]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Instrumental, Gender.M]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Instrumental, Gender.F]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Instrumental, Gender.N]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Instrumental, Gender.P]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Locative, Gender.M]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Locative, Gender.F]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Locative, Gender.N]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Case.Locative, Gender.P]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                /*v = adj[Comparison.Comparative1]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Comparison.Comparative2]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Comparison.Comparative3]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Comparison.Comparative4]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                                v = adj[Comparison.Comparative5]; if (!string.IsNullOrEmpty(v)) wfs.Add(v);*/
                            }
                        }
                        else
                        {
                            errors?.Add($"Похоже, что '{w.WordName}' не прилагательное");
                        }
                        break;
                    case ChatBotConst.WordType.Possessive:
                        v = Pronouns.Possessive(Case.Nominative, Person.First, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Nominative, Person.First, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Nominative, Person.First, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Genitive, Person.First, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Genitive, Person.First, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Genitive, Person.First, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Dative, Person.First, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Dative, Person.First, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Dative, Person.First, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Accusative, Person.First, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Accusative, Person.First, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Accusative, Person.First, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Instrumental, Person.First, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Instrumental, Person.First, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Instrumental, Person.First, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Locative, Person.First, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Locative, Person.First, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.Possessive(Case.Locative, Person.First, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        break;
                    case ChatBotConst.WordType.PossessiveReflexive:
                        v = Pronouns.PossessiveReflexive(Case.Nominative, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Nominative, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Nominative, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Genitive, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Genitive, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Genitive, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Dative, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Dative, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Dative, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Accusative, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Accusative, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Accusative, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Instrumental, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Instrumental, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Instrumental, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Locative, Gender.M); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Locative, Gender.F); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        v = Pronouns.PossessiveReflexive(Case.Locative, Gender.N); if (!string.IsNullOrEmpty(v)) wfs.Add(v);
                        break;
                }
            }

            return wfs.Distinct().ToList();
        }

        public PatternDto PatternCalculate(PatternDto pattern)
        {
            pattern.Phrase = pattern.Phrase.ToLower();
            var wordsSave = pattern.Words == null ? new List<WordDto>() : pattern.Words.ToList();
            var words = GetWordsFromPhrase(pattern.Phrase, false, true, false).Words;
            pattern.WordCount = words.Count;
            pattern.Words = new List<WordDto>();
            foreach (var w in words)
            {
                var wOld = wordsSave.FirstOrDefault(x => x.WordName == w);
                if (wOld == null)
                {
                    pattern.Words.Add(new WordDto { WordName = w, WordTypeId = -1, WordForms = new List<WordFormDto>() { new WordFormDto { Form = w } } });
                }
                else
                {
                    var wNew = new WordDto { WordName = w, WordTypeId = wOld.WordTypeId };
                    FillWordForms(wNew, null);
                    pattern.Words.Add(wNew);
                }
            }

            return pattern;
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

            return new WordListDto() { Words = res, IsDizzy = isDizzy };
        }

        public bool IsLetter(char c, bool isForTranslit)
        {
            var res = (c >= 'a' && c <= 'z') || (c >= 'а' && c <= 'я') || (c >= '0' && c <= '9') || c == '-' || c == '_' || c == 'ё' || c == '*';
            if (!res && isForTranslit)
                res = noDelimeters.Contains(c);
            return res;
        }



    }
}
