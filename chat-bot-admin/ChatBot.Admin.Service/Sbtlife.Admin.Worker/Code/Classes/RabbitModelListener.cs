using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.Common.Rabbit;
using ChatBot.Admin.Common.Rabbit.Abstractions;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.Worker.Code.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChatBot.Admin.Worker.Code.Classes
{
    public class RabbitModelListener : IRabbitModelListener
    {
        private readonly IModelOutConsumer _modelOutConsumer;
        private readonly IStoreLearnModelReportCommandHandler _storeLearnModelReportCommandHandler;
        private readonly ILogger<RabbitModelListener> _logger;
        private readonly IConfiguration _configuration;

        private bool _isRegistered = false;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public RabbitModelListener(
            IModelOutConsumer modelOutConsumer, 
            IStoreLearnModelReportCommandHandler storeLearnModelReportCommandHandler,
            IConfiguration configuration,
            ILogger<RabbitModelListener> logger)
        {
            _modelOutConsumer = modelOutConsumer;
            _storeLearnModelReportCommandHandler = storeLearnModelReportCommandHandler;
            _configuration = configuration;
            _logger = logger;
        }


        public  void Run()
        {
            Register();
             Task.Delay(Timeout.Infinite);
        }

        public void Register()
        {
            if (!_isRegistered)
            {
                if (_modelOutConsumer.Channel != null)
                {
                    var consumer = new EventingBasicConsumer(_modelOutConsumer.Channel);
                    consumer.Received += ReceiveModelOutEvent;
                    _modelOutConsumer.Channel.BasicConsume(queue: RabbitConst.RabbitQueueName.ModelOut, autoAck: true,
                        consumer: consumer, consumerTag: "", noLocal: true, exclusive: false, arguments: null);
                }

                _isRegistered = true;
            }
        }

        public void ReceiveModelOutEvent(object model, BasicDeliverEventArgs ea)
        {
            semaphoreSlim.Wait();
            try
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    if (Convert.ToBoolean(_configuration["Config:StoreRabbitAnswers"].ToLower())) {
                        _logger.LogWarning(message);
                        //File.WriteAllText("D:\\sbtlogs\\chatbot-admin-worker\\message.json", message);
                    }
                    var answer = JsonConvert.DeserializeObject<RabbitAnswerDto<LearningModelAnswerDto[]>>(message);

                    var command = new StoreLearnModelReportCommand()
                    {
                        ModelId = answer.task_id,
                        Report = answer.result[0],
                        FullAnswer = Convert.ToBoolean(_configuration["Config:StoreRabbitAnswers"].ToLower()) ? message : ""
                    };
                    command.SetId(Guid.NewGuid());
                    var res = _storeLearnModelReportCommandHandler.Handle(command);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            } catch (Exception e)
            {
                _logger.LogError(e, "RabbitModelListener.ReceiveModelOutEvent error");
            }
        }





        public void DeRegister()
        {
        }
    }
}

