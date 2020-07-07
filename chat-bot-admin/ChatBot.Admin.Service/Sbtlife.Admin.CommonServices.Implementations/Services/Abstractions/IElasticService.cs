using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IElasticService
    {
        void ReindexWords(List<WordIndexDto> words);
    }
}
