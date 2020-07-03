using System;
using AutoMapper;
using ChatBot.WebApp.Uavp.Const;
using ChatBot.WebApp.Uavp.ExceptionFilters;
using ChatBot.WebApp.Uavp.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SBoT.Code.Uavp.Classes;
using CodeBootstrapper = SBoT.Code.Uavp.ModuleBootstraper;
using DomainBootstrapper = SBoT.Domain.Uavp.ModuleBootstraper;

namespace ChatBot.WebApp.Uavp
{
    public class Startup
    {
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

        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            using (var loggerFactory = new LoggerFactory())
            {
                loggerFactory.ConfigureNLog($"nlog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.config");
                loggerFactory.AddNLog();

                var logger = loggerFactory.CreateLogger<Startup>();

                try
                {
                    services.AddSingleton(Configuration);
                    services.AddOptions();

                    services.AddAutoMapper();
                    services.AddMemoryCache();

                    services.AddOptions();

                    ConfigureUsers(services);

                    CodeBootstrapper.Configure(Configuration, services);
                    DomainBootstrapper.Configure(Configuration, services);

                    services
                        .AddMvc(o => { o.Filters.Add(typeof(GlobalExceptionFilterAttribute)); })
                        .AddJsonOptions(o => o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)
                        .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());

                    services.AddAuthentication(IISDefaults.AuthenticationScheme);
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                    throw;
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.ConfigureNLog($"nlog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.config");
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc();

        }


        private void ConfigureUsers(IServiceCollection services)
        {
            /*if (IsDevEnvironment())
            {
                var roles = new[] { new MockRole(RoleConst.Administrator, Guid.Empty, Guid.Empty) };
                services.AddMockUserService(new Guid("793b3458-dee6-4103-a3c9-6850c62ec504"), "Тестов Тест Тестович", "sigma\\test", "test@test.com", roles);
                return;
            }*/

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private bool IsDevEnvironment()
        {
            return HostingEnvironment.IsEnvironment(AppSettingsConst.AspNetAppDevEnvirontment);
        }
    }

    public static class ApplicationBuilderExtentions
    {
    }

}
