using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using ChatBot.Admin.WebApi.Helpers;
using ChatBot.Admin.WebApi.Mapping;
using ChatBot.Admin.WebApi.Requests;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
{
    [Route("chatbot")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChatBotCategoryController : Controller
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IGetCategoryCollection _getCollectionQuery;
        private readonly IGetChatBotLearningCollection _getChatBotLearningCollection;
        private readonly IGetChatBotPatternsCollection _getChatBotPatternsCollection;
        private readonly IGetChatBotModelCollection _getChatBotModelCollection;
        private readonly IGetCategoryItem _getItemQuery;
        private readonly IGetCategoryStat _getStatQuery;
        private readonly IGetCategoryXls _getStatXls;
        private readonly IGetPatternItem _getPatternQuery;
        private readonly IHostingEnvironment _env;

        public ChatBotCategoryController(IHostingEnvironment env, IPermissionsService permissionsService,
            IGetCategoryCollection getCollectionQuery,
            IGetChatBotLearningCollection getChatBotLearningCollection,
            IGetChatBotPatternsCollection getChatBotPatternsCollection,
            IGetChatBotModelCollection getChatBotModelCollection,
            IGetCategoryStat getStatQuery,
            IGetCategoryItem getItemQuery,
            IGetCategoryXls getStatXls,
            IGetPatternItem getPatternQuery)
        {
            _env = env;
            _permissionsService = permissionsService;
            _getCollectionQuery = getCollectionQuery;
            _getChatBotLearningCollection = getChatBotLearningCollection;
            _getChatBotPatternsCollection = getChatBotPatternsCollection;
            _getChatBotModelCollection = getChatBotModelCollection;
            _getItemQuery = getItemQuery;
            _getStatQuery = getStatQuery;
            _getStatXls = getStatXls;
            _getPatternQuery = getPatternQuery;
        }

        [HttpPost("collection")]
        public  IActionResult GetCollection([FromBody] GetCategoryCollectionFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetCategoryCollectionSpecification>(filter);
            var collection =  _getCollectionQuery.Ask(specification);
            var collectionDto = Mapper.Map<ViewModel.CollectionDto<CategoryDto>>(collection);

            return Json(collectionDto);
        }

        [HttpGet("{id}")]
        public  IActionResult GetItem(int id)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = new GetItemIntSpecification(id);
            var item =  _getItemQuery.Ask(specification);
            return Json(item);
        }

        [HttpGet("pattern/{id}")]
        public  IActionResult GetPatternItem(int id)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = new GetItemIntSpecification(id);
            var item =  _getPatternQuery.Ask(specification);
            return Json(item);
        }


        [HttpPost("learning")]
        public  IActionResult GetHistoryList([FromBody] GetLearningFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetLearningSpecification>(filter);
            var collection =  _getChatBotLearningCollection.Ask(specification);

            //var collectionDto = Mapper.Map<ViewModel.CollectionDto<LearningDto>>(collection);
            return Json(collection);
        }

        [HttpPost("patterns")]
        public  IActionResult GetPatternsList([FromBody] GetPatternsFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetPatternsSpecification>(filter);
            var collection =  _getChatBotPatternsCollection.Ask(specification);

            //var collectionDto = Mapper.Map<ViewModel.CollectionDto<LearningDto>>(collection);
            return Json(collection);
        }

        [HttpPost("model")]
        public  IActionResult GetModelList([FromBody] GetModelFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetModelSpecification>(filter);
            var collection =  _getChatBotModelCollection.Ask(specification);

            //var collectionDto = Mapper.Map<ViewModel.CollectionDto<LearningDto>>(collection);
            return Json(collection);
        }


        [HttpPost("stat")]
        public  IActionResult GetStat([FromBody] GetCategoryCollectionFilter filter)
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            var specification = Mapper.Map<GetCategoryCollectionSpecification>(filter);
            var res =  _getStatQuery.Ask(specification);

            return Json(res);
        }

        [HttpGet("xls")]
        public  IActionResult GetXls()
        {
            if (!_permissionsService.CanReadChatBot)
                throw new UnauthorizedAccessException();

            try
            {
                var file =  _getStatXls.Ask(new GetItemStringSpecification(_env.ContentRootPath));

                return HttpHelper.CreateResponseForFile(file);
            }
            catch (Exception e)
            {
                return HttpHelper.CreateResponseForError(e.Message);
            }
        }


    }
}
