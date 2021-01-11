using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SBoT.Code.Entity;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository;
using SBoT.Code.Repository.Interfaces;
using SBoT.Code.Services;
using SBoT.Code.Services.Abstractions;
using System;
using AutoMapper;
using SBoT.Code.Classes;
using SBoT.Code.Mapping;

namespace SBoT.Code
{
    public static class ModuleBootstraper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IChatter, Chatter>();
            serviceCollection.AddTransient<IChatterTransient, ChatterTransient>();
            serviceCollection.AddScoped<IWordFormer, WordFormer>();
            serviceCollection.AddScoped<IFileTransformer, FileTransformer>();
            serviceCollection.AddScoped<IRoleChecker, RoleChecker>();
            serviceCollection.AddScoped<IRabbitWorker, RabbitWorker>();
            serviceCollection.AddScoped<IWebRequestProcess, WebRequestProcess>();

            serviceCollection.AddSingleton<IRabbitListener, RabbitListener>();

            serviceCollection.AddScoped<IMessageHistoryService, MessageHistoryService>();
            serviceCollection.AddScoped<IRosterService, RosterService>();
            serviceCollection.AddScoped<IUserInfoService, UserInfoService>();
            serviceCollection.AddScoped<IInfoService, InfoService>();

            serviceCollection.AddScoped<ISboTRepository, SBoTRepository>();
            serviceCollection.AddTransient<ISboTRepositoryTransient, SBoTRepositoryTransient>();

            serviceCollection.AddScoped<ITimeMeasurer, TimeMeasurer>();

            RegisterBusDependencies(configuration, serviceCollection);

            serviceCollection.AddSingleton(provider =>
                new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper()
            );


            serviceCollection.AddScoped<IElasticWorker, ElasticWorker>();
        }

        public static void RegisterBusDependencies(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            var spellCheckInPublisher = new SpellCheckInPublisher();
            var spellCheckInConsumer = new SpellCheckInConsumer();
            var spellCheckOutPublisher = new SpellCheckOutPublisher();
            var spellCheckOutConsumer = new SpellCheckOutConsumer();
            var mtoInPublisher = new MtoInPublisher();
            var mtoOutConsumer = new MtoOutConsumer();
            var dictInPublisher = new DictInPublisher();

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

                var channel = connection.CreateModel();
                channel.QueueDeclare(queue: Const.RabbitQueueName.SpellCheckIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                spellCheckInPublisher.Channel = channel;

                channel = connection.CreateModel();
                channel.QueueDeclare(queue: Const.RabbitQueueName.CatalogIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                dictInPublisher.Channel = channel;

                if (Convert.ToBoolean(configuration["Config:UseOwnDecoder"].ToLower()))
                {
                    channel = connection.CreateModel();
                    channel.QueueDeclare(queue: Const.RabbitQueueName.SpellCheckIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    spellCheckInConsumer.Channel = channel;

                    channel = connection.CreateModel();
                    channel.QueueDeclare(queue: Const.RabbitQueueName.SpellCheckOut, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    spellCheckOutPublisher.Channel = channel;
                }

                if (Convert.ToBoolean(configuration["Config:ReadDecoded"].ToLower()))
                {
                    channel = connection.CreateModel();
                    channel.QueueDeclare(queue: Const.RabbitQueueName.SpellCheckOut, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    spellCheckOutConsumer.Channel = channel;
                }

                if (Convert.ToBoolean(configuration["Config:IsUseMto"].ToLower()))
                {
                    channel = connection.CreateModel();
                    channel.QueueDeclare(queue: Const.RabbitQueueName.MtoIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    mtoInPublisher.Channel = channel;
                }

                if (Convert.ToBoolean(configuration["Config:ReadDecodedMto"].ToLower()))
                {
                    channel = connection.CreateModel();
                    channel.QueueDeclare(queue: Const.RabbitQueueName.MtoOut, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    mtoOutConsumer.Channel = channel;
                }

            }

            serviceCollection.AddSingleton<ISpellCheckInPublisher>(spellCheckInPublisher);
            serviceCollection.AddSingleton<ISpellCheckInConsumer>(spellCheckInConsumer);
            serviceCollection.AddSingleton<ISpellCheckOutPublisher>(spellCheckOutPublisher);
            serviceCollection.AddSingleton<ISpellCheckOutConsumer>(spellCheckOutConsumer);
            serviceCollection.AddSingleton<IMtoInPublisher>(mtoInPublisher);
            serviceCollection.AddSingleton<IMtoOutConsumer>(mtoOutConsumer);
            serviceCollection.AddSingleton<IDictInPublisher>(dictInPublisher);
        }


    }
}
