using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Admin.WebApi.Controllers
{
    [Route("ping")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PingController : Controller
    {
        private static readonly string Version = Assembly.GetEntryAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        [HttpGet]
        public IActionResult Pong()
        {
            return Json(new { version = Version });
        }
    }
}
