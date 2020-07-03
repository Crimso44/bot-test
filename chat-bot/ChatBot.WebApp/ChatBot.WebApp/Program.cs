using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ChatBot.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSetting("detailedErrors", "true")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .Build();
    }
}
