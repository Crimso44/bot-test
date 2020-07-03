using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository.Interfaces;

namespace SBoT.Code.Entity
{
    public class RabbitListener : IRabbitListener
    {
        private readonly ISpellCheckInConsumer _scInConsumer;
        private readonly ISpellCheckOutConsumer _scOutConsumer;
        private readonly ISpellCheckOutPublisher _scOutPublisher;
        private readonly IMtoOutConsumer _mtoOutConsumer;
        private readonly ILogger<RabbitListener> _logger;

        private bool _isRegistered = false;
        private readonly Dictionary<Guid, string[]> _answers = new Dictionary<Guid, string[]>();
        private readonly Dictionary<Guid, RabbitMtoAnswerDto[]> _answersMto = new Dictionary<Guid, RabbitMtoAnswerDto[]>();
        private readonly ManualResetEvent oSignalEvent = new ManualResetEvent(true);
        private readonly ManualResetEvent oMtoSignalEvent = new ManualResetEvent(true);


        public RabbitListener(ISpellCheckInConsumer scInConsumer, ISpellCheckOutPublisher scOutPublisher, ISpellCheckOutConsumer scOutConsumer,
            IMtoOutConsumer mtoOutConsumer, ILogger<RabbitListener> logger)
        {
            _scInConsumer = scInConsumer;
            _scOutPublisher = scOutPublisher;
            _scOutConsumer = scOutConsumer;
            _mtoOutConsumer = mtoOutConsumer;
            _logger = logger;
        }


        public void Register()
        {
            if (!_isRegistered)
            {
                if (_scInConsumer.Channel != null)
                {
                    var consumer = new EventingBasicConsumer(_scInConsumer.Channel);
                    consumer.Received += ReceiveInEvent;
                    _scInConsumer.Channel.BasicConsume(queue: Const.RabbitQueueName.SpellCheckIn, autoAck: true, consumer: consumer, consumerTag: "", noLocal: true, exclusive: false, arguments: null);
                }

                if (_scOutConsumer.Channel != null)
                {
                    var consumer = new EventingBasicConsumer(_scOutConsumer.Channel);
                    consumer.Received += ReceiveOutEvent;
                    _scOutConsumer.Channel.BasicConsume(queue: Const.RabbitQueueName.SpellCheckOut, autoAck: true, consumer: consumer, consumerTag: "", noLocal: true, exclusive: false, arguments: null);
                }

                if (_mtoOutConsumer.Channel != null)
                {
                    var consumer = new EventingBasicConsumer(_mtoOutConsumer.Channel);
                    consumer.Received += ReceiveMtoOutEvent;
                    _mtoOutConsumer.Channel.BasicConsume(queue: Const.RabbitQueueName.MtoOut, autoAck: true, consumer: consumer, consumerTag: "", noLocal: true, exclusive: false, arguments: null);
                }

                _isRegistered = true;
            }
        }

        public void ReceiveInEvent(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            //Console.WriteLine(" [x] Received {0}", message);

            //var request = JsonConvert.DeserializeObject<SpellCheckInDto>(message);
            var arr = JsonConvert.DeserializeObject<object[]>(message);
            var reqstr = JsonConvert.SerializeObject(arr[0]);
            var request = JsonConvert.DeserializeObject<string[]>(reqstr);

            if (_scOutPublisher.Channel != null)
            {
                // отвечаем один-в-один, никаких спеллчекеров
                _scOutPublisher.SendMessageAnswer(Guid.Parse(ea.BasicProperties.CorrelationId), request);
                /*var bodyPublish = Encoding.UTF8.GetBytes(messagePublish);
                _scOutPublisher.Channel.BasicPublish(exchange: "", routingKey: Const.RabbitQueueName.SpellCheckOut, mandatory: false, basicProperties: null, body: bodyPublish);*/
            }

        }

        public void ReceiveOutEvent(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            //Console.WriteLine(" [x] Received {0}", message);

            var answer = JsonConvert.DeserializeObject<RabbitAnswerDto<string[]>>(message);
            lock (_answers)
            {
                _answers[answer.task_id] = answer.result;
            }
            oSignalEvent.Set();
        }

        public void ReceiveMtoOutEvent(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            //Console.WriteLine(" [x] Received {0}", message);

            //message = "{\"task_id\": \"591f1c33-75b4-4a02-b06b-925048b0bffa\", \"status\": \"SUCCESS\", \"result\": [{\"question\": \"\u0434\u0435\u0442\u0435\u0439\"}], \"traceback\": null, \"children\": []}";
            var answer = JsonConvert.DeserializeObject<RabbitAnswerDto<RabbitMtoAnswerDto[]>>(message);
            Regex rx = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");
            var msg = rx.Replace(message, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            if (answer?.result != null && answer.result.Any()) answer.result[0].message = msg;
            lock (_answersMto)
            {
                _answersMto[answer.task_id] = answer.result;
            }
            oMtoSignalEvent.Set();
        }


        public string[] GetAnswers(Guid id)
        {
            while (true)
            {
                lock (_answers)
                {
                    if (_answers.ContainsKey(id))
                    {
                        var res = _answers[id];
                        _answers.Remove(id);
                        return res;
                    }
                }

                oSignalEvent.Reset();
                if (!oSignalEvent.WaitOne(Const.RabbitMQ.WaitTimeout * 1000))
                    return null;
            }
        }


        public RabbitMtoAnswerDto[] GetMtoAnswers(Guid id)
        {
            while (true)
            {
                lock (_answersMto)
                {
                    if (_answersMto.ContainsKey(id))
                    {
                        var res = _answersMto[id];
                        _answersMto.Remove(id);
                        return res;
                    }
                }

                oMtoSignalEvent.Reset();
                if (!oMtoSignalEvent.WaitOne(Const.RabbitMQ.WaitTimeout * 1000))
                {
                    _logger.LogError("No answer from Machine Learning");
                    return null;
                }
            }
        }


        public void DeRegister()
        {
            if (_scInConsumer.Channel != null)
            {

            }
        }
    }
}
