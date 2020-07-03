using System;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SBoT.Domain.Const;
using SBoT.Domain.DataModel.SBoT;
using SBoT.Domain.DataModel.SBoT.Interfaces;

namespace SBoT.Domain
{
    public static class ModuleBootstraper
    {
        public static void AddElasticSearch(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var uri = new Uri(configuration[AppSettingsConst.ConnectionStrings.ElasticSearch]);
            var connectionSettings = new ConnectionSettings(uri);
            var transport = new Transport<ConnectionSettings>(connectionSettings);
            transport.Settings.ThrowExceptions();
            var elasticClient = new ElasticClient(transport);
            serviceCollection.AddSingleton<IElasticClient>(elasticClient);
        }


        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<SBoTDataModel>(o => o.UseSqlServer(configuration[AppSettingsConst.ConnectionStrings.SBot]));
            serviceCollection.AddScoped<ISBoTDataModel>(provider => provider.GetRequiredService<SBoTDataModel>());
            serviceCollection.AddDbContext<SBoTDataModelTransient>(o => o.UseSqlServer(configuration[AppSettingsConst.ConnectionStrings.SBot]), ServiceLifetime.Transient);
            serviceCollection.AddTransient<ISBoTDataModelTransient>(provider => provider.GetRequiredService<SBoTDataModelTransient>());

            serviceCollection.AddElasticSearch(configuration);
        }
    }
}
