using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.CommonServices.Rabbit.Abstractions
{
    public interface IRabbitListener
    {
        void Register();
        void DeRegister();
        string GetAnswers(Guid id);
    }
}
