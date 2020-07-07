using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;
using ChatBot.Admin.ReadStorage.Extensions;
using ChatBot.Admin.ReadStorage.Mapping;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotChangersDictionaryCollection : QueryBase, IGetChatBotChangersDictionaryCollection
    {
        private readonly IChatBotReadonlyContext _context;
        private readonly IChatInfoService _chatInfoService;

        public GetChatBotChangersDictionaryCollection(ILogger<GetChatBotChangersDictionaryCollection> logger,
            IChatBotReadonlyContext context, IChatInfoService chatInfoService)
            : base(logger)
        {
            _context = context;
            _chatInfoService = chatInfoService;
        }

        public  CollectionDto<DictionaryStringItemDto> Ask(GetCollectionSpecification specification)
        {
            var changerLogins =  _context.Categories.Where(x => !string.IsNullOrEmpty(x.ChangedBy)).Select(x => x.ChangedBy).Distinct().ToList();
            if (!changerLogins.Any()) return null;

            var changers = _chatInfoService.GetUsersInfo(changerLogins);
            var res = changers.Select(x => $"{x.Name}#$#$#{x.SigmaLogin}").Distinct().OrderBy(x => x).Select(x =>
            {
                var data = x.Split("#$#$#");
                return new DictionaryStringItemDto {Id = data[1], Title = data[0]};
            }).ToList();

            return new CollectionDto<DictionaryStringItemDto>
            {
                Count = res.Count,
                Items = res
            };
        }
    }
}
