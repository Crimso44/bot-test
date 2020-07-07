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
    class GetPatternItem : QueryBase, IGetPatternItem
    {
        private readonly IChatBotReadonlyContext _context;

        public GetPatternItem(ILogger<GetPatternItem> logger,
            IChatBotReadonlyContext context)
            : base(logger)
        {
            _context = context;
        }

        public  PatternDto Ask(GetItemIntSpecification specification)
        {
            CheckSpecificationIsNotNullOrThrow(specification);

            var req =  (
                from p in _context.Patterns
                from pw in _context.PatternWordRels.Where(pw => p.Id == pw.PatternId).DefaultIfEmpty()
                from w in _context.Words.Where(w => pw.WordId == w.Id).DefaultIfEmpty()
                from wf in _context.WordForms.Where(wf => w.Id == wf.WordId).DefaultIfEmpty()
                where p.Id == specification.Id
                select new { p, pw, w, wf }).ToList();

            PatternDto pat = Mapper.Map<PatternDto>(req[0].p);
            pat.Words = new List<WordDto>();
            WordDto word = null;
            WordFormDto wfm = null;
            foreach (var d in req)
            {
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

            return pat;

        }
    }
}
