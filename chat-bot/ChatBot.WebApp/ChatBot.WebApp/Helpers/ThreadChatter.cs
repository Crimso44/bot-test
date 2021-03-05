using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;

namespace ChatBot.WebApp.Helpers
{
    public class ThreadChatter
    {
        private readonly IChatter _chatter;
        private List<ReportDto> _data;

        private ThreadChatterCallback _callback;

        public ThreadChatter(IChatter chatter, List<ReportDto> data, ThreadChatterCallback callback)
        {
            _chatter = chatter;
            _data = data;
            _callback = callback;
        }

        public void ThreadProc()
        {
            var res01 = new List<ReportMtoDto>();
            var cnt = 0;
            foreach (var d in _data)
            {
                cnt++;
                if (!d.OriginalQuestion.StartsWith("("))
                {
                    var answer1 = _chatter.AskBotEx("Rep", d.OriginalQuestion, d.ContextIn, null, true, true, false);
                    res01.Add(new ReportMtoDto()
                    {
                        Question = d.OriginalQuestion,
                        IsMtoAnswer = answer1.IsMtoAnswer,
                        OriginalCategorys = answer1.OriginalCategorys,
                        ModelResponse = answer1.ModelResponse
                    });
                    Debug.Write($"{cnt} from {_data.Count} ... {d.OriginalQuestion}\n");
                }
            }
            _callback(res01);
        }
    }

    public delegate void ThreadChatterCallback(List<ReportMtoDto> result);
}
