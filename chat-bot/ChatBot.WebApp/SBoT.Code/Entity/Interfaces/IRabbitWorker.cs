using System;
using System.Collections.Generic;
using System.Text;
using SBoT.Code.Dto;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IRabbitWorker
    {
        bool SendQuestion(Guid id, List<string> words);
        bool SendMtoQuestion(Guid id, List<string> words);
        List<string> ReceiveAnswers(Guid id);
        List<RabbitMtoAnswerDto> ReceiveMtoAnswers(Guid id);
        void SendDictionary();
    }
}
