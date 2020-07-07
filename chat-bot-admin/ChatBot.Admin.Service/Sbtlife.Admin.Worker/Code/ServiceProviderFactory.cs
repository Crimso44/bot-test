using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ChatBot.Admin.CommandHandlers;
using ChatBot.Admin.CommonServices;
using ChatBot.Admin.DomainStorage;
using ChatBot.Admin.DomainStorage.Mapping;
using ChatBot.Admin.Worker.Code.UserManagementMock;
using Um.Connect.Mock;

//using Um.Connect.Mock;

namespace ChatBot.Admin.Worker.Code
{
    internal class ServiceProviderFactory
    {
        public IServiceProvider Build()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine)}.json", optional: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            NLog.LogManager.LoadConfiguration($"nlog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine)}.config");

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog();

            services.AddSingleton<ILoggerFactory>(loggerFactory);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            var logger = loggerFactory.CreateLogger<ServiceProviderFactory>();

            try
            {
                services.AddMockUserService(new SystemUser());

                //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                //services.AddHttpContextIdentityProvider();
                //services.AddSqlServerUserService();

                //ConfigureUsers(services);
                WorkerModuleBootstrapper.Configure(configuration, services);
                CommandModuleBootstraper.Configure(configuration, services);
                DomainModuleBootstraper.Configure(configuration, services);
                CommonModuleBootstraper.Configure(configuration, services);

                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<DomainAutoMapperProfile>();
                });
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                throw;
            }

            var provider = services.BuildServiceProvider();
            return provider;
        }

        /*private void ConfigureUsers(IServiceCollection services)
        {
            services.AddMockUserService(new AppUser());
        }*/
    }
}
