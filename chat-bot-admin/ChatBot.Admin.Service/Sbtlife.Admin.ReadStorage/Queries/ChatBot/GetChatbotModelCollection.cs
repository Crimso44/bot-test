using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Extensions;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotModelCollection : QueryBase, IGetChatBotModelCollection
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotModelCollection(ILogger<GetChatBotModelCollection> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  CollectionDto<ModelLearningDto> Ask(GetModelSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);
            specification.AdjustSearch();
            specification.AdjustSkipTake();

            var query = _context.ModelLearnings.AsQueryable();

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
                query = query.OrderByDescending(i => i.CreateDate);
            }

            if (specification.Skip != null)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null)
                query = query.Take(specification.Take.Value);


            var items =  query.ToArray();


            var itemIds = items.Select(x => x.Id).ToList();
            var reports =  _context.ModelLearningReports.Where(x => itemIds.Contains(x.ModelLearningId)).ToList();
            var confusions =  (
                from conf in _context.ModelLearningConfs
                join cat in _context.Categories on new {conf.CategoryId, IsTest = true} equals new {CategoryId = cat.OriginId, IsTest = cat.IsTest ?? false} into ccat
                from cat in ccat.DefaultIfEmpty()
                join catTo in _context.Categories on new { CategoryId = conf.ToCategoryId, IsTest = true } equals new { CategoryId = catTo.OriginId, IsTest = catTo.IsTest ?? false } into ccatTo
                from catTo in ccatTo.DefaultIfEmpty()
                where itemIds.Contains(conf.ModelLearningId)
                select new {conf, cat, catTo}).ToList();
            var markups =  _context.ModelLearningMarkups.Where(x => itemIds.Contains(x.ModelLearningId)).ToList();


            return new CollectionDto<ModelLearningDto>
            {
                Count = count,
                Items = items.Select(x =>
                {
                    var res = Mapper.Map<ModelLearningDto>(x);
                    res.Report = reports.Where(y => y.ModelLearningId == res.Id).Select(Mapper.Map<ModelReportDto>).ToList();
                    foreach (var rep in res.Report)
                    {
                        rep.ConfusionFrom = confusions
                            .Where(y => y.conf.ModelLearningId == res.Id && y.conf.CategoryId == rep.CategoryId)
                            .OrderBy(y => y.conf.CategoryId == y.conf.ToCategoryId ? 0 : 1)
                            .ThenByDescending(y => y.conf.Confusion)
                            .Take(10)
                            .Select(y =>
                                new ModelReportConfusionDto
                                {
                                    OriginId = y.conf.ToCategoryId,
                                    CategoryName = y.conf.ToCategoryName,
                                    Confusion = Convert.ToInt32(y.conf.Confusion ?? 0),
                                    CategoryId = y.catTo?.Id,
                                    Questions = markups
                                        .Where(z => z.ModelLearningId == res.Id && z.CategoryFrom == y.conf.CategoryId && z.CategoryTo == y.conf.ToCategoryId)
                                        .Select(z => z.Question).ToList()
                                }
                            ).ToList();
                        rep.ConfusionTo = confusions
                            .Where(y => y.conf.ModelLearningId == res.Id && y.conf.ToCategoryId == rep.CategoryId)
                            .OrderBy(y => y.conf.CategoryId == y.conf.ToCategoryId ? 0 : 1)
                            .ThenByDescending(y => y.conf.Confusion)
                            .Take(10)
                            .Select(y =>
                                new ModelReportConfusionDto()
                                {
                                    OriginId = y.conf.CategoryId,
                                    CategoryName = y.conf.CategoryName,
                                    Confusion = Convert.ToInt32(y.conf.Confusion ?? 0),
                                    CategoryId = y.cat?.Id,
                                    Questions = markups
                                        .Where(z => z.ModelLearningId == res.Id && z.CategoryFrom == y.conf.CategoryId && z.CategoryTo == y.conf.ToCategoryId)
                                        .Select(z => z.Question).ToList()
                                }
                            ).ToList();
                    }
                    return res;
                }).ToList()
            };
        }
    }
}

