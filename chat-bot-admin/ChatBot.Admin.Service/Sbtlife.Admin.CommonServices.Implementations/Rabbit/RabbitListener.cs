using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using ChatBot.Admin.Common.Rabbit;
using ChatBot.Admin.Common.Rabbit.Abstractions;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.CommonServices.Rabbit.Abstractions;

namespace ChatBot.Admin.CommonServices.Rabbit
{
    public class RabbitListener : IRabbitListener
    {
        private readonly ISpellCheckOutConsumer _scOutConsumer;

        private bool _isRegistered = false;
        private readonly Dictionary<Guid, string> _answers = new Dictionary<Guid, string>();
        private readonly ManualResetEvent oSignalEvent = new ManualResetEvent(true);


        public RabbitListener(ISpellCheckOutConsumer scOutConsumer)
        {
            _scOutConsumer = scOutConsumer;
        }


        public void Register()
        {
            if (!_isRegistered)
            {
                if (_scOutConsumer.Channel != null)
                {
                    var consumer = new EventingBasicConsumer(_scOutConsumer.Channel);
                    consumer.Received += ReceiveOutEvent;
                    _scOutConsumer.Channel.BasicConsume(queue: RabbitConst.RabbitQueueName.SpellCheckOut, autoAck: true, consumer: consumer, consumerTag: "", noLocal: true, exclusive: false, arguments: null);
                }

                _isRegistered = true;
            }
        }

        public void ReceiveOutEvent(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            //Console.WriteLine(" [x] Received {0}", message);

            var answer = JsonConvert.DeserializeObject<RabbitAnswerDto<string>>(message);
            lock (_answers)
            {
                _answers[answer.task_id] = answer.result;
            }
            oSignalEvent.Set();
        }


        public string GetAnswers(Guid id)
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
                if (!oSignalEvent.WaitOne(RabbitConst.RabbitMQ.WaitTimeout * 1000))
                    return null;
            }
        }




        public void DeRegister()
        {
        }
    }
}
