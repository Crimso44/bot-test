using Newtonsoft.Json;
using RabbitMQ.Client;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SBoT.Code.Classes;
using SBoT.Code.Repository.Interfaces;

namespace SBoT.Code.Entity
{
    public class RabbitWorker : IRabbitWorker
    {
        private readonly ISpellCheckInPublisher _scInPublisher;
        private readonly IMtoInPublisher _mtoInPublisher;
        private readonly IDictInPublisher _dictInPublisher;
        private readonly IRabbitListener _rabbitListener;
        private readonly ISboTRepository _sbotRepository;
        private readonly IWordFormer _wordFormer;

        private readonly Random rnd = new Random();

        public RabbitWorker(ISpellCheckInPublisher scInPublisher, IDictInPublisher dictInPublisher, 
            IRabbitListener rabbitListener, ISboTRepository sbotRepository, IWordFormer wordFormer, IMtoInPublisher mtoInPublisher)
        {
            _scInPublisher = scInPublisher;
            _mtoInPublisher = mtoInPublisher;
            _rabbitListener = rabbitListener;
            _sbotRepository = sbotRepository;
            _wordFormer = wordFormer;
            _dictInPublisher = dictInPublisher;

            _rabbitListener.Register();
        }

        public bool SendQuestion(Guid id, List<string> words)
        {
            if (_scInPublisher.Channel != null)
            {
                var dto = new string[] {string.Join(" ", words) };
                _scInPublisher.SendMessage(id, dto);
                return true;
            }

            return false;
        }

        public bool SendMtoQuestion(Guid id, List<string> words)
        {
            if (_mtoInPublisher.Channel != null)
            {
                var dto = new string[] { string.Join(" ", words) };
                _mtoInPublisher.SendMessage(id, dto);
                return true;
            }

            return false;
        }

        public List<string> ReceiveAnswers(Guid id)
        {
            var answer = _rabbitListener.GetAnswers(id);
            return answer?.ToList();
        }

        public List<RabbitMtoAnswerDto> ReceiveMtoAnswers(Guid id)
        {
            var answerMto = _rabbitListener.GetMtoAnswers(id);
            return answerMto?.ToList();
        }

        public void SendDictionary()
        {
            if (_dictInPublisher.Channel != null)
            {
                var dto = _sbotRepository.GetCategoriesWords().ToArray();
                _dictInPublisher.SendMessage(Guid.NewGuid(), dto);
            }
        }

    }
}
