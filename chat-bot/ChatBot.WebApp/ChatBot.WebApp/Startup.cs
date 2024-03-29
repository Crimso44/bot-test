﻿using System;
using System.Text;
using AutoMapper;
using ChatBot.WebApp.Const;
using ChatBot.WebApp.ExceptionFilters;
using ChatBot.WebApp.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using SBoT.Code;
using SBoT.Code.Classes;
using SBoT.Code.Entity;
using CodeBootstrapper = SBoT.Code.ModuleBootstraper;
using DomainBootstrapper = SBoT.Domain.ModuleBootstraper;

namespace ChatBot.WebApp
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

                    services.Configure<Config>(Configuration.GetSection("Config"));
                    services.Configure<Urls>(Configuration.GetSection("Urls"));
                    services.AddOptions();

                    ConfigureUsers(services);

                    CodeBootstrapper.Configure(Configuration, services);
                    DomainBootstrapper.Configure(Configuration, services);

                    services.AddCors(options =>
                        options.AddPolicy("SiteCorsPolicy", builder => builder
                            .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials())
                    );
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

            app.UseCors("SiteCorsPolicy");
            app.UseAuthentication();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

            app.UseRabbitListener();
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
        public static RabbitListener Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            //Listener = app.ApplicationServices.GetService<RabbitListener>();

            //var life = app.ApplicationServices.GetService<IApplicationLifetime>();

            //life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            //life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Register();
        }

        private static void OnStopping()
        {
            Listener.DeRegister();
        }
    }

}
