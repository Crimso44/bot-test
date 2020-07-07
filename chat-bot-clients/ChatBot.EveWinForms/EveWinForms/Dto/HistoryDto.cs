using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eve
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public DateTime QuestionDate { get; set; }
        public string SigmaLogin { get; set; }
        public string UserName { get; set; }
        public string Question { get; set; }
        public string OriginalQuestion { get; set; }
        public string Answer { get; set; }
        public string AnswerText { get; set; }
        public string AnswerType { get; set; }
        public decimal? Rate { get; set; }
        public string SetContext { get; set; }
        public string Context { get; set; }
        public bool IsButton { get; set; }
        public short? Like { get; set; }
        public Guid? CategoryOriginId { get; set; }
        public bool? IsMto { get; set; }
        public string MtoThresholds { get; set; }
    }
}
