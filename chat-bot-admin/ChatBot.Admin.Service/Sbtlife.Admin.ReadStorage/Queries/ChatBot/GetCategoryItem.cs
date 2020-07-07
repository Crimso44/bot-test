using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Queries.ChatBot
{
    class GetCategoryItem : QueryBase, IGetCategoryItem
    {
        private readonly IChatBotReadonlyContext _context;
        private readonly IChatInfoService _chatInfoService;

        public GetCategoryItem(ILogger<GetCategoryItem> logger, IChatInfoService chatInfoService,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
            _chatInfoService = chatInfoService;
        }

        public  CategoryDto Ask(GetItemIntSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);

            var req =  (
                from c in _context.Categories
                from p in _context.Patterns.Where(p => c.Id == p.CategoryId).DefaultIfEmpty()
                from pw in _context.PatternWordRels.Where(pw => p.Id == pw.PatternId).DefaultIfEmpty()
                from w in _context.Words.Where(w => pw.WordId == w.Id).DefaultIfEmpty()
                from wf in _context.WordForms.Where(wf => w.Id == wf.WordId).DefaultIfEmpty()
                from pt in _context.Partitions.Where(pt => pt.Id == c.PartitionId).DefaultIfEmpty()
                from ptt in _context.Partitions.Where(ptt => ptt.Id == pt.ParentId).DefaultIfEmpty()
                where c.Id == specification.Id
                select new { c, p, pw, w, wf, pt, ptt }).ToList();

            var res = Mapper.Map<CategoryDto>(req[0].c);
            if (req[0].pt != null)
            {
                res.Partition = Mapper.Map<PartitionDto>(req[0].pt);
                res.UpperPartition = Mapper.Map<PartitionDto>(req[0].ptt);
            }

            res.RequiredRosterName = _chatInfoService.RosterName(req[0].c.RequiredRoster);
            res.Patterns = new List<PatternDto>();

            PatternDto pat = null;
            WordDto word = null;
            WordFormDto wfm = null;
            foreach (var d in req)
            {
                if (d.p != null)
                {
                    if (pat == null || pat.Id != d.p.Id)
                    {
                        pat = Mapper.Map<PatternDto>(d.p);
                        pat.Words = new List<WordDto>();
                        res.Patterns.Add(pat);
                        word = null;
                        wfm = null;
                    }

                    if (d.w != null)
                    {
                        if (word == null || word.Id != d.w.Id)
                        {
                            word = Mapper.Map<WordDto>(d.w);
                            word.WordForms = new List<WordFormDto>();
                            pat.Words.Add(word);
                            wfm = null;
                        }

                        if (d.wf != null && (wfm == null || wfm.Id != d.wf.Id))
                        {
                            wfm = Mapper.Map<WordFormDto>(d.wf);
                            word.WordForms.Add(wfm);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(res.ChangedBy))
            {
                var usr = _chatInfoService.GetUserInfo(res.ChangedBy);
                if (!string.IsNullOrEmpty(usr.Name))
                    res.ChangedByName = usr.Name;
            }

            res.Patterns = res.Patterns.OrderBy(x => x.Phrase).ToList();

            //var learnings =  (_context.Learning.Where(x => x.CategoryId == res.OriginId).ToList());
            //res.Learnings = learnings.Select(Mapper.Map<LearningDto>).ToList();

            return res;

        }
    }
}
