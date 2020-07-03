using System;

namespace SBoT.Code.Dto
{
    public class ReportDto
    {
        public DateTime Date { get; set; }
        public string FIO { get; set; }
        public string Payload { get; set; }
        public string TabNo { get; set; }
        public string Context { get; set; }
        public string ContextIn { get; set; }
        public string Question { get; set; }
        public string OriginalQuestion { get; set; }
        public bool IsAnswered { get; set; }
        public string Answer { get; set; }
        public string AnswerText { get; set; }
        public System.Int16 Like { get; set; }
        public Guid? CategoryOriginId { get; set; }
        public bool? IsMto { get; set; }
        public string Partition { get; set; }
        public string SubPartition { get; set; }
        public string MtoThresholds { get; set; }
        public string SigmaLogin { get; set; }
        public string Source { get; set; }
    }
}
