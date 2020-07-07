using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using ChatBot.Admin.WebApi.Mapping;
using ChatBot.Admin.WebApi.Requests;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
{
    [Route("chatbotpartitions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChatBotPartitionController : Controller
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IGetChatBotPartitionCollection _getCollectionQuery;
        private readonly IGetChatBotPartitionItem _getItemQuery;

        public ChatBotPartitionController(IPermissionsService permissionsService,
            IGetChatBotPartitionCollection getCollectionQuery,
            IGetChatBotPartitionItem getItemQuery)
        {
            _permissionsService = permissionsService;
            _getCollectionQuery = getCollectionQuery;
            _getItemQuery = getItemQuery;
        }

        [HttpPost("collection")]
        public  IActionResult GetCollection([FromBody] GetCategoryCollectionFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetCategoryCollectionSpecification>(filter);
            var collection =  _getCollectionQuery.Ask(specification);

            var collectionDto = Mapper.Map<ViewModel.CollectionDto<Common.Model.ChatBot.PartitionDto>>(collection);
            return Json(collectionDto);
        }

        [HttpGet("{id}")]
        public  IActionResult GetItem(Guid id)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = new GetItemSpecification(id);
            var item =  _getItemQuery.Ask(specification);
            return Json(item);
        }
    }
}
