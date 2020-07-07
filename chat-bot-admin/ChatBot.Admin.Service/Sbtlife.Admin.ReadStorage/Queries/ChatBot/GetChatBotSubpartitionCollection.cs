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
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotSubpartitionCollection : QueryBase, IGetChatBotSubpartitionCollection
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotSubpartitionCollection(ILogger<GetChatBotSubpartitionCollection> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  CollectionDto<PartitionDto> Ask(GetCategoryCollectionSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);
            specification.AdjustSearch();
            specification.AdjustSkipTake();

            var query = _context.Partitions.AsQueryable().Where(x => x.ParentId != null);

            if (specification.HasSearch())
                query = query.Where(i => i.Title.Contains(specification.Search));

            if (specification.PartitionId.HasValue)
                query = query.Where(i => i.ParentId == specification.PartitionId.Value);

            var count =  query.Count()
                ;

            query = query
                .OrderBy(i => i.Title);

            if (specification.Skip != null)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null)
                query = query.Take(specification.Take.Value);

            var items =  query
                .ToArray()
                ;

            return new CollectionDto<PartitionDto>
            {
                Count = count,
                Items = items.Select(Mapper.Map<PartitionDto>)
            };
        }
    }
}

