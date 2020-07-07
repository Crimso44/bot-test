using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetCategoryStat : QueryBase, IGetCategoryStat
    {
        private readonly IChatBotReadonlyContext _context;

        public GetCategoryStat(ILogger<GetCategoryStat> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  string Ask(GetCategoryCollectionSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);

            var res = "";
            var req = _context.Categories.Where(x => x.IsTest ?? false);
            if (specification.SubPartitionId.HasValue)
            {
                req = req.Where(x => x.PartitionId == specification.SubPartitionId.Value);
            }
            else if (specification.PartitionId.HasValue)
            {
                req = (
                    from c in req
                    join p in _context.Partitions on c.PartitionId equals p.Id
                    where p.ParentId == specification.PartitionId.Value
                    select c);
            }
            var changed =  req.Count(x => x.IsChanged ?? false);
            var added =  req.Count(x => x.IsAdded ?? false);
            if (added > 0) res += $"Добавлено вопросов: {added}; ";
            if (changed > 0) res += $"Изменено вопросов: {changed}; ";
            return res;

        }
    }
}
