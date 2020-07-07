using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Extensions;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetCategoryCollection : QueryBase, IGetCategoryCollection
    {
        private readonly IChatBotReadonlyContext _context;
        private readonly IChatInfoService _chatInfoService;

        public GetCategoryCollection(ILogger<GetCategoryCollection> logger, IChatInfoService chatInfoService,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
            _chatInfoService = chatInfoService;
        }

        public  CollectionDto<CategoryDto> Ask(GetCategoryCollectionSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);
            specification.AdjustSearch();
            specification.AdjustSkipTake();


            var query = (
                from c in _context.Categories
                from pt in _context.Partitions.Where(pt => pt.Id == c.PartitionId).DefaultIfEmpty()
                //from p in _context.Patterns.Where(p => c.Id == p.CategoryId).DefaultIfEmpty()
                where c.IsTest ?? false
                select new {c, pt});
                /*group new { c, p, pt } by new { c, pt } into g
                orderby g.Key.c.Id
                select new
                {
                    cat = g.Key,
                    pat = g.ToList()
                })*/;
            if (!string.IsNullOrEmpty(specification.Search))
                query = query.Where(x => x.c.Name.ToLower().Contains(specification.Search.ToLower()));
            if (!string.IsNullOrEmpty(specification.Answer))
                query = query.Where(x => x.c.Response.ToLower().Contains(specification.Answer.ToLower()));
            if (!string.IsNullOrEmpty(specification.Pattern))
            {
                var lIds = (
                    from l in _context.Learnings 
                    where l.Question.ToLower().Contains(specification.Pattern.ToLower())
                    select l.CategoryId);
                var pIds = (
                    from p in _context.Patterns
                    where p.Phrase.Contains(specification.Pattern.ToLower())
                    select p.CategoryId);
                query = query.Where(x => pIds.Contains(x.c.Id) || lIds.Contains(x.c.OriginId));
            }

            if (specification.IsDisabled)
            {
                query = query.Where(x => x.c.IsDisabled ?? false);
            }

            if (!string.IsNullOrEmpty(specification.Context))
            {
                var pIds = (
                    from p in _context.Patterns
                    where !string.IsNullOrEmpty(p.Context) && p.Context.ToLower().Contains(specification.Context.ToLower())
                    select p.CategoryId);
                query = query.Where(x =>
                    (!string.IsNullOrEmpty(x.c.SetContext) && x.c.SetContext.ToLower().Contains(specification.Context.ToLower())) ||
                    pIds.Contains(x.c.Id));
            }

            if (specification.SubPartitionId.HasValue)
                query = query.Where(x => x.pt.Id == specification.SubPartitionId.Value);
            if (specification.PartitionId.HasValue)
                query = query.Where(x => x.pt.ParentId == specification.PartitionId.Value);
            if (!string.IsNullOrEmpty(specification.ChangedBy))
                query = query.Where(x => x.c.ChangedBy == specification.ChangedBy);

            var count =  query.Count()
                ;

            switch (specification.SortColumn)
            {
                case "razd":
                    query = specification.SortDescent
                        ? query.OrderByDescending(x => x.pt.FullTitle.ToLower())
                        : query.OrderBy(x => x.pt.FullTitle.ToLower());
                    break;
                case "cat":
                    query = specification.SortDescent
                        ? query.OrderByDescending(x => x.c.Name.ToLower())
                        : query.OrderBy(x => x.c.Name.ToLower());
                    break;
                /*case "pat":
                    query = specification.SortDescent
                        ? query.OrderByDescending(x =>
                            x.pat != null && x.pat.Any() ? string.Join("", x.pat.Select(y => y.p == null ? "" : y.p.Phrase).ToList()) : "")
                        : query.OrderBy(x =>
                            x.pat != null && x.pat.Any() ? string.Join("", x.pat.Select(y => y.p == null ? "" : y.p.Phrase).ToList()) : "");
                    break;*/
                case "answ":
                    query = specification.SortDescent
                        ? query.OrderByDescending(x => x.c.Response.ToLower())
                        : query.OrderBy(x => x.c.Response.ToLower());
                    break;
                /*case "cont":
                    query = specification.SortDescent
                        ? query.OrderByDescending(x =>
                            (string.IsNullOrEmpty(x.cat.c.SetContext) ? "" : x.cat.c.SetContext.ToLower()) + string.Join("", x.pat.Select(y => (y.p == null || string.IsNullOrEmpty(y.p.Context)) ? "" : y.p.Context.ToLower()).ToList()))
                        : query.OrderBy(x =>
                            (string.IsNullOrEmpty(x.cat.c.SetContext) ? "" : x.cat.c.SetContext.ToLower()) + string.Join("", x.pat.Select(y => (y.p == null || string.IsNullOrEmpty(y.p.Context)) ? "" : y.p.Context.ToLower()).ToList()));
                    break;*/
                case "chng":
                    query = specification.SortDescent
                        ? query.OrderByDescending(x => x.c.ChangedOn)
                        : query.OrderBy(x => x.c.ChangedOn);
                    break;
            }

            if (specification.Skip != null)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null)
                query = query.Take(specification.Take.Value);

            var items =  query
                .ToArray()
                ;

            var res = items.Select(x =>
            {
                var c = Mapper.Map<CategoryDto>(x.c);
                if (x.pt != null)
                    c.Partition = new PartitionDto { Id = x.pt.Id, Title = x.pt.FullTitle };
                return c;
            }).ToList();

            var cIds = res.Select(x => x.Id).ToList();
            var pats = _context.Patterns.Where(x => cIds.Contains(x.CategoryId)).ToList();

            foreach (var c in res)
            {
                c.Patterns = pats.Where(x => x.CategoryId == c.Id).Select(y => Mapper.Map<PatternDto>(y)).ToList();
                c.RequiredRosterName = _chatInfoService.RosterName(c.RequiredRoster);
            }


            var logins = res.Where(x => !string.IsNullOrEmpty(x.ChangedBy)).Select(x => x.ChangedBy).Distinct().ToList();
            if (logins.Any())
            {
                var usrs = _chatInfoService.GetUsersInfo(logins);
                if (usrs.Any())
                {
                    foreach (var cat in res)
                    {
                        var usr = usrs.FirstOrDefault(x => x.SigmaLogin == cat.ChangedBy);
                        if (!string.IsNullOrEmpty(usr?.Name))
                            cat.ChangedByName = usr?.Name;
                    }
                }
            }


            return new CollectionDto<CategoryDto>
            {
                Count = count,
                Items = res
            };

        }
    }
}
