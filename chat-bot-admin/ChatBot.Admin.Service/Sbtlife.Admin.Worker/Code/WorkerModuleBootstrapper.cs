using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.Common.Rabbit;
using ChatBot.Admin.Common.Rabbit.Abstractions;
using ChatBot.Admin.Worker.Code.Classes;
using ChatBot.Admin.Worker.Code.Interfaces;

namespace ChatBot.Admin.Worker.Code
{
    public static class WorkerModuleBootstrapper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            var modelOutConsumer = new ModelOutConsumer();
            if (Convert.ToBoolean(configuration["Config:IsRabbitMQ"].ToLower()))
            {
                var url = Environment.GetEnvironmentVariable("ConnectionStrings:MessageBroker",
                    EnvironmentVariableTarget.Machine);
                //url = "amqp://rmq-test:giYZPy7&@sbt-dup-005:5672"; 

                if (url == null)
                    throw new ArgumentException("Не найден параметр ConnectionStrings:MessageBroker");

                url = url.Replace("rabbitmq:", "amqp:");

                var factory = new ConnectionFactory() {Uri = new Uri(url)};
                var connection = factory.CreateConnection();

                if (Convert.ToBoolean(configuration["Config:ReadDecodedModel"].ToLower()))
                {
                    var channel = connection.CreateModel();
                    channel.QueueDeclare(queue: RabbitConst.RabbitQueueName.ModelOut, durable: true, exclusive: false,
                        autoDelete: false, arguments: null);
                    modelOutConsumer.Channel = channel;
                }
            }

            serviceCollection.AddSingleton<IModelOutConsumer>(modelOutConsumer);
            serviceCollection.AddSingleton<IRabbitModelListener, RabbitModelListener>();
        }
    }
}
