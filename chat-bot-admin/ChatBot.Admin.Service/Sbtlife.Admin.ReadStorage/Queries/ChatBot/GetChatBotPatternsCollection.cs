using AutoMapper;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Extensions;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotPatternsCollection : QueryBase, IGetChatBotPatternsCollection
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotPatternsCollection(ILogger<GetChatBotPatternsCollection> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  CollectionDto<PatternsDto> Ask(GetPatternsSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);
            specification.AdjustSearch();
            specification.AdjustSkipTake();

            var query = _context.Patterns.Where(x => x.IsTest ?? false);

            if (specification.HasSearch())
                query = query.Where(i => i.Phrase.Contains(specification.Search));

            if (specification.Category != null && specification.Category.Any())
            {
                var cats = specification.Category.Select(x => x.Id).ToList();
                query = query.Where(x => cats.Contains(x.CategoryId));
            }

            if (specification.PartitionId.HasValue)
            {
                query = query.Where(x => x.CategoryPartitionId == specification.PartitionId);
            }
            else if (specification.UpperPartitionId.HasValue)
            {
                query = query.Where(x => x.CategoryUpperPartitionId == specification.UpperPartitionId);
            }

            if (specification.Filtering != null && specification.Filtering.Length > 0)
            {
                query = query.Where($"{specification.Filtering[0].Id}.Contains(@0)", specification.Filtering[0].Value);
                for (var i = 1; i < specification.Filtering.Length; i++)
                {
                    query = query.Where($"{specification.Filtering[i].Id}.Contains(@0)", specification.Filtering[i].Value);
                }
            }


            var count =  query.Count()
                ;

            if (specification.Sorting != null && specification.Sorting.Length > 0)
            {
                var sort = specification.Sorting[0].Id + (specification.Sorting[0].Desc ? " desc" : "");
                for (var i = 1; i < specification.Sorting.Length; i++)
                {
                    sort += ", " + specification.Sorting[i].Id + (specification.Sorting[i].Desc ? " desc" : "");
                }
                query = query.OrderBy(sort).ThenBy(x => x.Phrase);
            }
            else
            {
                query = query.OrderByDescending(i => i.Phrase);
            }

            if (specification.Skip != null)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null)
                query = query.Take(specification.Take.Value);

            var items =  query
                .ToArray()
                ;

            return new CollectionDto<PatternsDto>
            {
                Count = count,
                Items = items.Select(Mapper.Map<PatternsDto>).ToList()
            };
        }
    }
}
