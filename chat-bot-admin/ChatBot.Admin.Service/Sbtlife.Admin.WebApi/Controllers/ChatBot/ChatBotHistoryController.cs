using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.Common.Const.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using ChatBot.Admin.WebApi.Mapping;
using ChatBot.Admin.WebApi.Requests;
using ChatBot.Admin.WebApi.ViewModel;
using ChatBot.Admin.WebApi.ViewModel.ChatBot;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
{
    [Route("chatbothistory")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChatBotHistoryController : Controller
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IGetChatBotHistoryCollection _getChatBotHistoryCollection;

        public ChatBotHistoryController(IPermissionsService permissionsService,
            IGetChatBotHistoryCollection getChatBotHistoryCollection)
        {
            _permissionsService = permissionsService;
            _getChatBotHistoryCollection = getChatBotHistoryCollection;
        }

        [HttpPost("list")]
        public  IActionResult GetHistoryList([FromBody] GetHistoryFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetHistorySpecification>(filter);
            var collection =  _getChatBotHistoryCollection.Ask(specification);

            var collectionDto = Mapper.Map<ViewModel.CollectionDto<ChatBotHistoryDto>>(collection);
            return Json(collectionDto);
        }
    }
}
