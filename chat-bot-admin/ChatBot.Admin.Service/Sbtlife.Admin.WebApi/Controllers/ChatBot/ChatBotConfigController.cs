using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
{
    [Route("chatbotconfig")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChatBotConfigController : Controller
    {
        private readonly IGetChatBotSettingsCollection _getSettingsCollection;

        public ChatBotConfigController(IGetChatBotSettingsCollection getSettingsCollection)
        {
            _getSettingsCollection = getSettingsCollection;
        }

        [HttpGet("settings")]
        public  IActionResult GetSettings()
        {
            var settings =  _getSettingsCollection.Ask(null);

            return Json(settings);
        }
    }
}
