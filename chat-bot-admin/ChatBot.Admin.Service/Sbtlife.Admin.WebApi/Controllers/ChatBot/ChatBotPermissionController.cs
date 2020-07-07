using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.WebApi.ViewModel.ChatBot;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
{
    [Route("chatbotuser")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChatBotPermissionController : Controller
    {
        private readonly IPermissionsService _permissionsService;

        public ChatBotPermissionController(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        [HttpGet("permissions")]
        public IActionResult GetPermissions()
        {
            var permissions = new ChatBotPermissionDto
            {
                CanEditChatBot = _permissionsService.CanEditChatBot,
                CanReadChatBot = _permissionsService.CanReadChatBot
            };

            return Json(permissions);
        }
    }
}
