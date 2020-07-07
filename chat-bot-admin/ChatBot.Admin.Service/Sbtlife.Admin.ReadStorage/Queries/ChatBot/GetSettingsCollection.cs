using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;
using ChatBot.Admin.ReadStorage.Extensions;
using ChatBot.Admin.ReadStorage.Mapping;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using System.Collections.Generic;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotSettingsCollection : QueryBase, IGetChatBotSettingsCollection
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotSettingsCollection(ILogger<GetCategoryCollection> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  Dictionary<string, string> Ask(GetItemSpecification specification)
        {
            var data =  _context.Configs.ToDictionary(x => x.Name, x => x.Value);
            return data;
        }
    }
}
