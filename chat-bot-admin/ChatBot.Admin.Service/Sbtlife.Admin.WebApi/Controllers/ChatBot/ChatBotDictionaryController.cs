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
using ChatBot.Admin.WebApi.Mapping;
using ChatBot.Admin.WebApi.Requests;
using ChatBot.Admin.WebApi.ViewModel;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
    {
        [Route("chatbotdictionary")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public class ChatBotDictionaryController : Controller
        {
            private readonly IPermissionsService _permissionsService;
            private readonly IChatInfoService _chatInfoService;
            private readonly IGetChatBotPartitionDictionaryCollection _getPartitionCollectionQuery;
            private readonly IGetChatBotSubpartitionDictionaryCollection _getSubpartitionCollectionQuery;
            private readonly IGetChatBotChangersDictionaryCollection _getChangersQuery;

        public ChatBotDictionaryController(IPermissionsService permissionsService,
                IChatInfoService chatInfoService,
                IGetChatBotPartitionDictionaryCollection getPartitionCollectionQuery,
                IGetChatBotSubpartitionDictionaryCollection getSubpartitionCollectionQuery,
                IGetChatBotChangersDictionaryCollection getChangersQuery)
            {
                _permissionsService = permissionsService;
                _chatInfoService = chatInfoService;
                _getPartitionCollectionQuery = getPartitionCollectionQuery;
                _getSubpartitionCollectionQuery = getSubpartitionCollectionQuery;
                _getChangersQuery = getChangersQuery;
            }

            [HttpPost("partitions")]
            public  IActionResult GetPartitionCollection([FromBody] GetCollectionFilter filter)
            {
                if (!_permissionsService.CanReadChatBot)
                    throw new UnauthorizedAccessException();

                var specification = Mapper.Map<GetCollectionSpecification>(filter);
                var collection =  _getPartitionCollectionQuery.Ask(specification);

                var collectionDto = Mapper.Map<ViewModel.CollectionDto<ViewModel.DictionaryItemDto>>(collection);
                return Json(collectionDto);
            }

            [HttpPost("subpartitions")]
            public  IActionResult GetSubpartitionCollection([FromBody] GetCollectionFilter filter)
            {
                if (!_permissionsService.CanReadChatBot)
                    throw new UnauthorizedAccessException();

                var specification = Mapper.Map<GetCollectionSpecification>(filter);
                var collection =  _getSubpartitionCollectionQuery.Ask(specification);

                var collectionDto = Mapper.Map<ViewModel.CollectionDto<ViewModel.DictionaryItemDto>>(collection);
                return Json(collectionDto);
            }

            [HttpGet("changers")]
            public  IActionResult GetChangerList()
            {
                if (!_permissionsService.CanReadChatBot)
                    throw new UnauthorizedAccessException();

                var collection =  _getChangersQuery.Ask(null);

                var collectionDto = Mapper.Map<ViewModel.CollectionDto<ViewModel.DictionaryStringItemDto>>(collection);
                return Json(collectionDto);
            }

            [HttpGet("sources")]
            public  IActionResult GetSourcesList()
            {
                if (!_permissionsService.CanReadChatBot)
                    throw new UnauthorizedAccessException();

                var collectionDto = new ViewModel.CollectionDto<ViewModel.DictionaryStringItemDto>()
                {
                    Count = _chatInfoService.Roster().Keys.Count,
                    Items = _chatInfoService.Roster().Values.Select(x => new ViewModel.DictionaryStringItemDto() { Id = x.Id, Title = x.Name }).ToList()
                };
                return Json(collectionDto);
            }
        }
}
