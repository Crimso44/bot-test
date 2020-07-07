using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;
using ChatBot.Admin.ReadStorage.Mapping;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetCategoryXls : QueryBase, IGetCategoryXls
    {
        private readonly IChatBotReadonlyContext _context;
        private readonly IFileTransformService _fileTransformer;

        public GetCategoryXls(ILogger<GetCategoryXls> logger, IChatBotReadonlyContext context, IFileTransformService fileTransformer)
            : base(logger)
        {
            _context = context;
            _fileTransformer = fileTransformer;
        }

        public  FileDto Ask(GetItemStringSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);

            var data =  (
                from c in _context.Categories
                from p in _context.Patterns.Where(p => c.Id == p.CategoryId).DefaultIfEmpty()
                from pw in _context.PatternWordRels.Join(_context.Words, pw => pw.WordId, w => w.Id, (pw, w) => new { pw, w }).Where(x => p.Id == x.pw.PatternId && x.w.WordTypeId.HasValue).DefaultIfEmpty()
                from pt in _context.Partitions.Where(pt => pt.Id == c.PartitionId).DefaultIfEmpty()
                from ptt in _context.Partitions.Where(ptt => ptt.Id == pt.ParentId).DefaultIfEmpty()
                where c.IsTest ?? false
                group new { c, p, pw.w, pt, ptt } by new { c, pt, ptt } into g
                orderby g.Key.c.Id
                select new
                {
                    k = g.Key,
                    p = g.ToList()
                }
            ).ToList();
            var res = data.Select(x =>
            {
                var c = Mapper.Map<CategoryDto>(x.k.c);
                if (x.k.pt != null)
                    c.Partition = new PartitionDto { Id = x.k.pt.Id, ParentId = x.k.pt.ParentId, Title = x.k.pt.Title };
                if (x.k.ptt != null)
                    c.UpperPartition = new PartitionDto { Id = x.k.ptt.Id, ParentId = x.k.ptt.ParentId, Title = x.k.ptt.Title };
                c.Patterns =
                    x.p.GroupBy(
                        pw => pw.p,
                        pw => new { pw.p, pw.w },
                        (p, gg) => new { p, w = gg.Select(y => y.w).ToList() }
                    ).ToList()
                    .Select(y =>
                    {
                        var p = Mapper.Map<PatternDto>(y.p);
                        if (p != null)
                            p.Words = y.w.Where(w => w != null).Select(Mapper.Map<WordDto>).ToList();
                        return p;
                    }).ToList();
                c.Learnings = _context.Learnings.Where(y => y.CategoryId == c.OriginId).Select(Mapper.Map<LearningDto>).ToList();
                return c;
            }
            ).ToArray();

            var bytes = _fileTransformer.MakeXls(res, specification.Id);
            return new FileDto { Body = bytes, Name = "SBoT.xlsx" };
        }
    }
}
