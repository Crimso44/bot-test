using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using RabbitMQ.Client;
using ChatBot.Admin.Common.Rabbit;
using ChatBot.Admin.Common.Rabbit.Abstractions;
using ChatBot.Admin.CommonServices.Rabbit;
using ChatBot.Admin.CommonServices.Rabbit.Abstractions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.CommonServices.Services;

namespace ChatBot.Admin.CommonServices
{
    public static class CommonModuleBootstraper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRoleService, RoleService>();
            serviceCollection.AddScoped<IPermissionsService, PermissionsService>();
            serviceCollection.AddScoped<IDateTimeService, DateTimeService>();
            serviceCollection.AddSingleton<IChatInfoService, ChatInfoService>();
            serviceCollection.AddSingleton<IJsonSerializerService, JsonSerializerService>();
            serviceCollection.AddSingleton<IImageService, ImageService>();
            serviceCollection.AddSingleton<IWordService, WordService>();
            serviceCollection.AddSingleton<IFileTransformService, FileTransformService>();
            serviceCollection.AddSingleton<IElasticService, ElasticService>();
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<ITempCache, TempCache>();
            serviceCollection.AddScoped<IWebRequestProcess, WebRequestProcess>();


            var dictInPublisher = new DictInPublisher();
            var scInPublisher = new SpellCheckInPublisher();
            var scOutConsumer = new SpellCheckOutConsumer();
            var modelInPublisher = new ModelInPublisher();

            if (Convert.ToBoolean(configuration["Config:IsRabbitMQ"].ToLower()))
            {
                var url = Environment.GetEnvironmentVariable("ConnectionStrings:MessageBroker", EnvironmentVariableTarget.Machine);
                //url = "amqp://rmq-test:giYZPy7&@sbt-dup-005:5672"; 

                if (url == null)
                    throw new ArgumentException("Не найден параметр ConnectionStrings:MessageBroker");

                url = url.Replace("rabbitmq:", "amqp:");

                var factory = new ConnectionFactory() { Uri = new Uri(url) };
                var connection = factory.CreateConnection();

                var channel = connection.CreateModel();
                channel.QueueDeclare(queue: RabbitConst.RabbitQueueName.CatalogIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                dictInPublisher.Channel = channel;

                channel = connection.CreateModel();
                channel.QueueDeclare(queue: RabbitConst.RabbitQueueName.SpellCheckIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                scInPublisher.Channel = channel;

                channel = connection.CreateModel();
                channel.QueueDeclare(queue: RabbitConst.RabbitQueueName.ModelIn, durable: true, exclusive: false, autoDelete: false, arguments: null);
                modelInPublisher.Channel = channel;

                if (Convert.ToBoolean(configuration["Config:ReadDecoded"].ToLower()))
                {
                    channel = connection.CreateModel();
                    channel.QueueDeclare(queue: RabbitConst.RabbitQueueName.SpellCheckOut, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    scOutConsumer.Channel = channel;
                }
            }


            serviceCollection.AddSingleton<IRabbitWorker, RabbitWorker>();
            serviceCollection.AddSingleton<IRabbitListener, RabbitListener>();
            serviceCollection.AddSingleton<IDictInPublisher>(dictInPublisher);
            serviceCollection.AddSingleton<ISpellCheckInPublisher>(scInPublisher);
            serviceCollection.AddSingleton<ISpellCheckOutConsumer>(scOutConsumer);
            serviceCollection.AddSingleton<IModelInPublisher>(modelInPublisher);

            serviceCollection.AddElasticSearch(configuration);
        }

        public static void AddElasticSearch(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var uri = new Uri(configuration["ConnectionStrings:ElasticSearch"]);
            if (uri == null)
                throw new ArgumentException("Не найден параметр ConnectionStrings:ElasticSearch");
            var connectionSettings = new ConnectionSettings(uri);
            var transport = new Transport<ConnectionSettings>(connectionSettings);
            transport.Settings.ThrowExceptions();
            var elasticClient = new ElasticClient(transport);
            serviceCollection.AddSingleton<IElasticClient>(elasticClient);
        }



    }
}
