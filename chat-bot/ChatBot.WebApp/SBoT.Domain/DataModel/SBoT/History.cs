using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("History")]
    public class History
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

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Rate { get; set; }

        public string SetContext { get; set; }
        public string Context { get; set; }

        public bool IsButton { get; set; }
        public System.Int16? Like { get; set; }
        public Guid? CategoryOriginId { get; set; }
        public bool? IsMto { get; set; }
        public string MtoThresholds { get; set; }
        public string Source { get; set; }
    }
}
