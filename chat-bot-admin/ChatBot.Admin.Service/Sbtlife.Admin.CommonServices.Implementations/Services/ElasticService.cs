using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommonServices.Services
{
    public class ElasticService : IElasticService
    {
        public const string SearchIndexName = "sbt-life-chatbot-search";

        private readonly IElasticClient _elasticClient;

        public ElasticService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }


        public void ReindexWords(List<WordIndexDto> words)
        {
            _elasticClient.DeleteByQuery<WordIndexDto>(del => del
                .Index(SearchIndexName)
                .Query(q => q.QueryString(qs => qs.Query("*")))
            );
            _elasticClient.IndexMany(words, SearchIndexName);
        }
    }
}
