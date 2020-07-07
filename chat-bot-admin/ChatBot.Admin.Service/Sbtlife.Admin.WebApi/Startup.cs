using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ChatBot.Admin.Common.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.WebApi.ExceptionFilters;
using ChatBot.Admin.WebApi.Mapping;
using ChatBot.Admin.WebApi.UserManagementMock;
using Um.Abstractions.Core.Const;
using Um.Connect.IdentityProviders;
using Um.Connect.Mock;
using Um.Connect.SqlServer;
using CommandHandlersBootstrapper = ChatBot.Admin.CommandHandlers.CommandModuleBootstraper;
using CommonServicesBootstrapper = ChatBot.Admin.CommonServices.CommonModuleBootstraper;
using DomainStorageBootstrapper = ChatBot.Admin.DomainStorage.DomainModuleBootstraper;
using ReadStorageBootstrapper = ChatBot.Admin.ReadStorage.ReadModuleBootstraper;

namespace ChatBot.Admin.WebApi
{
    public class Startup
    {
        private IContainer Container { get; set; }
        private IHostingEnvironment HostingEnvironment { get; }
        private IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            using (var loggerFactory = new LoggerFactory())
            {
                loggerFactory.AddNLog();

                var logger = loggerFactory.CreateLogger<Startup>();

                try
                {
                    services.AddSingleton<IConfiguration>(Configuration);
                    services.AddOptions();
                    services.AddMemoryCache();

                    services.Configure<Urls>(Configuration.GetSection("Urls"));

                    ConfigureUsers(services);
                    AddCors(services);

                    CommandHandlersBootstrapper.Configure(Configuration, services);
                    CommonServicesBootstrapper.Configure(Configuration, services);
                    DomainStorageBootstrapper.Configure(Configuration, services);
                    ReadStorageBootstrapper.Configure(Configuration, services);

                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>();
                        cfg.AddProfile<DomainStorage.Mapping.DomainAutoMapperProfile>();
                        cfg.AddProfile<ReadStorage.Mapping.ReadAutoMapperProfile>();
                    });

                    services
                        .AddMvc(o => { o.Filters.Add(typeof(GlobalExceptionFilterAttribute)); })
                        .AddJsonOptions(o => { o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });

                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.Populate(services);

                    Container = containerBuilder.Build();

                    return new AutofacServiceProvider(Container);
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                    throw;
                }
            }
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            loggerFactory.ConfigureNLog($"nlog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.config");
            loggerFactory.AddNLog();

            UseCors(app);
            app.UseMvc();

            applicationLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }

        private void ConfigureUsers(IServiceCollection services)
        {
            if (IsDevEnvironment())
            {
                var roles = new[] { new MockRole(
                    //Um.Abstractions.ChatBot.RoleConst.ChatBotReports, ScopeConst.SberbankTechnology, Um.Abstractions.ChatBot.ApplicationConst.ChatBot
                    Um.Abstractions.ChatBot.RoleConst.ChatBotAdministrator, ScopeConst.SberbankTechnology, Um.Abstractions.ChatBot.ApplicationConst.ChatBot
                ) };
                services.AddMockUserService(new Guid("793b3458-dee6-4103-a3c9-6850c62ec504"), "Тестов Тест Тестович", "sigma\\test", "test@test.com", roles);
                return;
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextIdentityProvider();
            services.AddSqlServerUserService();
        }

        private void AddCors(IServiceCollection services)
        {
            if (!IsDevOrTestEnvironment())
                return;

            services.AddCors();
        }

        private void UseCors(IApplicationBuilder app)
        {
            if (!IsDevOrTestEnvironment())
                return;

            app.UseCors(builder => builder
                .WithOrigins("http://localhost:9000")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
        }

        private bool IsDevOrTestEnvironment()
        {
            return IsDevEnvironment()
                   || IsTestEnvironment();
        }

        private bool IsDevEnvironment()
        {
            return HostingEnvironment.IsEnvironment(AppSettingsConst.AspNetAppDevEnvirontment);
        }

        private bool IsTestEnvironment()
        {
            return HostingEnvironment.IsEnvironment(AppSettingsConst.AspNetAppTestEnvirontment);
        }
    }
}
