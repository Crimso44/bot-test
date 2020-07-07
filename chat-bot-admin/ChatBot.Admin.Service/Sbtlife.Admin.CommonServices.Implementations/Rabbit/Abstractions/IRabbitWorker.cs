using System;
using System.Collections.Generic;
using ChatBot.Admin.Common.Rabbit.Model;

namespace ChatBot.Admin.CommonServices.Rabbit.Abstractions
{
    public interface IRabbitWorker
    {
        void SendDictionary(Guid id, WordListOutDto[] data);
        bool SendQuestion(Guid id, List<string> words);
        bool SendModelCommand(Guid id, string command, string data);
        string ReceiveAnswers(Guid id);
    }
}
