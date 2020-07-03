using System;
using System.Collections.Generic;
using System.Text;
using SBoT.Code.Dto;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IRabbitListener
    {
        void Register();
        void DeRegister();
        string[] GetAnswers(Guid id);
        RabbitMtoAnswerDto[] GetMtoAnswers(Guid id);
    }
}
