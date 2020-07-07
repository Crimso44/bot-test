using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;
using ChatBot.Admin.ReadStorage.Mapping;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using System.Linq;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotSubpartitionItem : QueryBase, IGetChatBotSubpartitionItem
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotSubpartitionItem(ILogger<GetChatBotSubpartitionItem> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  PartitionDto Ask(GetItemSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);

            var item =  _context.Partitions
                .SingleOrDefault(i => i.Id == specification.Id)
                ;

            return Mapper.Map<PartitionDto>(item);
        }
    }
}
