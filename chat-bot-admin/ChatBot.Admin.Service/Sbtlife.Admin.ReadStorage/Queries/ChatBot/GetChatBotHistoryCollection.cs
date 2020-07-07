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
using ChatBot.Admin.Common.Classes;
using Microsoft.Extensions.Options;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using System;
using System.Collections.Generic;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetChatBotHistoryCollection : QueryBase, IGetChatBotHistoryCollection
    {
        private readonly IChatBotReadonlyContext _context;

        public GetChatBotHistoryCollection(ILogger<GetChatBotHistoryCollection> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  CollectionDto<HistoryDto> Ask(GetHistorySpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);
            specification.AdjustSearch();
            specification.AdjustSkipTake();

            var query = _context.Histories.AsQueryable();

            if (specification.HasSearch())
                query = query.Where(i => i.Question.Contains(specification.Search));

            if (specification.From.HasValue)
            {
                query = query.Where(x => x.QuestionDate >= specification.From.Value);
            }

            if (specification.To.HasValue)
            {
                query = query.Where(x => x.QuestionDate <= specification.To.Value.AddDays(1));
            }

            if (specification.CategoryOriginId.HasValue)
            {
                query = query.Where(x => x.CategoryOriginId == specification.CategoryOriginId);
            }

            if (specification.Filtering != null && specification.Filtering.Length > 0)
            {
                foreach (var filter in specification.Filtering)
                {
                    query = query.Where($"{filter.Id}.Contains(@0)", filter.Value);
                }
            }

            if (specification.IsAnsweredMl.HasValue && !specification.IsAnsweredMl.Value)
            {
                query = query.Where(x => !((x.IsMto ?? false) && x.CategoryOriginId.HasValue && !x.IsButton));
            }

            if (specification.IsAnsweredEve.HasValue && !specification.IsAnsweredEve.Value)
            {
                query = query.Where(x => !(!(x.IsMto ?? false) && x.CategoryOriginId.HasValue && !x.IsButton));
            }

            if (specification.IsAnsweredButton.HasValue && !specification.IsAnsweredButton.Value)
            {
                query = query.Where(x => !x.IsButton);
            }

            if (specification.IsAnsweredNo.HasValue && !specification.IsAnsweredNo.Value)
            {
                query = query.Where(x => x.AnswerType != "NoAnswer");
            }

            if (specification.IsAnsweredOther.HasValue && !specification.IsAnsweredOther.Value)
            {
                query = query.Where(x => x.CategoryOriginId.HasValue || x.IsButton || x.AnswerType == "NoAnswer");
            }

            if (specification.IsLikeYes.HasValue && !specification.IsLikeYes.Value)
            {
                query = query.Where(x => !((x.Like ?? 0) > 0));
            }

            if (specification.IsDisLike.HasValue && !specification.IsDisLike.Value)
            {
                query = query.Where(x => !((x.Like ?? 0) < 0));
            }

            if (specification.IsLikeNo.HasValue && !specification.IsLikeNo.Value)
            {
                query = query.Where(x => x.Like.HasValue && x.Like != 0);
            }

            if (specification.IsMlYes.HasValue && !specification.IsMlYes.Value)
            {
                query = query.Where(x => !x.LearnId.HasValue || x.LearnCategoryId.HasValue);
            }

            if (specification.IsMlAnswer.HasValue && !specification.IsMlAnswer.Value)
            {
                query = query.Where(x => !x.LearnId.HasValue || !x.LearnCategoryId.HasValue || !x.AnswerGood);
            }

            if (specification.IsMlWrong.HasValue && !specification.IsMlWrong.Value)
            {
                query = query.Where(x => !x.LearnId.HasValue || !x.LearnCategoryId.HasValue || x.AnswerGood);
            }

            if (specification.IsMlNo.HasValue && !specification.IsMlNo.Value)
            {
                query = query.Where(x => x.LearnId.HasValue);
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
                query = query.OrderByDescending(i => i.QuestionDate);
            }

            if (specification.Skip != null)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null)
                query = query.Take(specification.Take.Value);

            var items =  query
                .ToArray()
                ;

            var res = new CollectionDto<HistoryDto>
            {
                Count = count,
                Items = items.Select(Mapper.Map<HistoryDto>).ToList()
            };

            var historyIds = items.Select(x => x.Id).ToList();
            var learnings =  
                (from l in _context.Learnings
                 join h in _context.Histories on l.Question equals h.Question.Trim()
                 where historyIds.Contains(h.Id)
                 group l by h.Id into g
                 select new { HistoryId = g.Key, Learnings = g.ToList() }
            ).ToList();
            foreach(var l in learnings)
            {
                var h = res.Items.Single(x => x.Id == l.HistoryId);
                h.Learns = l.Learnings.Select(Mapper.Map<LearningDto>).ToList();
            }

            return res;
        }
    }
}

