using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository.Interfaces;

namespace SBoT.Code.Entity
{
    public class ElasticWorker : IElasticWorker
    {
        public const string SearchIndexName = "sbt-life-chatbot-search";

        private readonly IElasticClient _elasticClient;
        private readonly ISboTRepository _sboTRepository;

        public ElasticWorker(IElasticClient elasticClient, ISboTRepository sboTRepository)
        {
            _elasticClient = elasticClient;
            _sboTRepository = sboTRepository;
        }

        public void ReindexWords()
        {
            _elasticClient.DeleteByQuery<WordIndexDto>(del => del
                .Index(SearchIndexName)
                .Query(q => q.QueryString(qs => qs.Query("*")))
            );
            var words = _sboTRepository.GetWordsForIndex();
            _elasticClient.IndexMany(words, SearchIndexName);
        }

        public List<ResponseDto> FindResponse(List<string> words, string context)
        {
            var fixedWords = _sboTRepository.GetFixedWords();
            var found = new List<WeightDto>();
            foreach (var word in words)
            {
                var isKeyword = word.Length < Const.Elastic.MinLetterCount || word.StartsWith("_") || fixedWords.Contains(word);
                ISearchResponse<WordIndexDto> fnd;
                if (isKeyword)
                {
                    fnd = _elasticClient.Search<WordIndexDto>(t => t
                        .Index(SearchIndexName)
                        .Query(q => q
                            .Ids(f => f.Values(word))
                        )
                    );
                }
                else
                {
                    fnd = _elasticClient.Search<WordIndexDto>(t => t
                        .Index(SearchIndexName)
                        .Query(q => q
                            .Fuzzy(f => f.Field("id").Value(word).Fuzziness(Fuzziness.EditDistance(1)).Transpositions(true))
                        )
                    );
                }

                var weights = (
                    from hit in fnd.Hits
                    from pattern in hit.Source.Pattern
                    group new {score = isKeyword ? 1 : hit.Score ?? 0, word = hit.Source.Id} by pattern into g
                    select new WeightDto
                    {
                        Id = g.Key,
                        Weight = isKeyword ? 1 : g.Max(x => x.score) / 10,
                        Word = word,
                        Words = (isKeyword ? g : g.Where(y => y.score == g.Max(x => x.score))).Select(y => y.word).ToList()
                    }
                ).ToList();
                found.AddRange(weights);
            }

            return _sboTRepository.FindResponseByWeights(found, words.Count, context);
        }

    }
}
