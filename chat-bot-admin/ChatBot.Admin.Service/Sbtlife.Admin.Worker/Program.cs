using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Worker.Code;
using ChatBot.Admin.Worker.Code.Classes;
using ChatBot.Admin.Worker.Code.Interfaces;

namespace ChatBot.Admin.Worker
{
    class Program
    {

        static void Main(string[] args)
        {
            var logger = GlobalServiceProvider.Instance.GetRequiredService<ILogger<Program>>();
            var runner = GlobalServiceProvider.Instance.GetRequiredService<IRabbitModelListener>();

            try
            {
                logger.LogTrace($"Chatbot worker started at {DateTime.Now:G}");
                runner.Run();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }
    }
}