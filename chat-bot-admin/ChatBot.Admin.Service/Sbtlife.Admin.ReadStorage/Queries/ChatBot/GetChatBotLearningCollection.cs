using System.Linq;
using System.Linq.Dynamic.Core;
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
using ChatBot.Admin.ReadStorage.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotLearningCollection : QueryBase, IGetChatBotLearningCollection
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotLearningCollection(ILogger<GetChatBotLearningCollection> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  CollectionDto<LearningDto> Ask(GetLearningSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);
            specification.AdjustSearch();
            specification.AdjustSkipTake();

            var query = _context.Learnings.AsQueryable();

            if (specification.HasSearch())
                query = query.Where(i => i.Question.Contains(specification.Search));

            if (specification.Category != null && specification.Category.Any())
            {
                var cats = specification.Category.Select(x => x.Id).ToList();
                query = query.Where(x => x.CategoryId.HasValue && cats.Contains(x.CategoryId.Value));
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
                query = query.OrderBy(sort);
            }
            else
            {
                query = query.OrderByDescending(i => i.Question);
            }

            if (specification.Skip != null)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null)
                query = query.Take(specification.Take.Value);

            var items =  query
                .ToArray()
                ;

            return new CollectionDto<LearningDto>
            {
                Count = count,
                Items = items.Select(Mapper.Map<LearningDto>).ToList()
            };
        }
    }
}

