using System;
using System.Collections.Generic;
using ChatBot.Admin.Common.Rabbit.Abstractions;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.CommonServices.Rabbit.Abstractions;

namespace ChatBot.Admin.CommonServices.Rabbit
{
    public class RabbitWorker : IRabbitWorker
    {
        private readonly IDictInPublisher _dictInPublisher;
        private readonly ISpellCheckInPublisher _scInPublisher;
        private readonly IModelInPublisher _modelInPublisher;
        private readonly IRabbitListener _rabbitListener;

        private readonly Random rnd = new Random();

        public RabbitWorker(IDictInPublisher dictInPublisher, ISpellCheckInPublisher scInPublisher, IModelInPublisher modelInPublisher, IRabbitListener rabbitListener)
        {
            _dictInPublisher = dictInPublisher;
            _scInPublisher = scInPublisher;
            _modelInPublisher = modelInPublisher;
            _rabbitListener = rabbitListener;

            _rabbitListener.Register();
        }


        public void SendDictionary(Guid id, WordListOutDto[] data)
        {
            if (_dictInPublisher.Channel != null)
            {
                _dictInPublisher.SendMessage(id, data);
            }
        }

        public bool SendQuestion(Guid id, List<string> words)
        {
            if (_scInPublisher.Channel != null)
            {
                var dto = new string[] { string.Join(" ", words) };
                _scInPublisher.SendMessage(id, dto);
                return true;
            }

            return false;
        }

        public bool SendModelCommand(Guid id, string command, string data)
        {
            if (_modelInPublisher.Channel != null)
            {
                var cmd = new ModelCommandDto() {Command = command, Data = data};
                var dto = new ModelCommandDto[] { cmd };
                _modelInPublisher.SendMessage(id, dto);
                return true;
            }

            return false;
        }


        public string ReceiveAnswers(Guid id)
        {
            var answer = _rabbitListener.GetAnswers(id);
            return answer;
        }


    }
}
