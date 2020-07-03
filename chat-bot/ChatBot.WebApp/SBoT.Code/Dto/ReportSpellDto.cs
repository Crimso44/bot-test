using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Dto
{
    public class ReportSpellDto
    {
        public string OriginalQuestion { get; set; }
        public string QuestionFirst { get; set; }
        public string AnswerFirst { get; set; }
        public string QuestionSecond { get; set; }
        public string AnswerSecond { get; set; }
        public bool? IsMto { get; set; }
    }
}
