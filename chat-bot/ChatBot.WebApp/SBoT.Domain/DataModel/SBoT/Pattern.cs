using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("Pattern")]
    public class Pattern
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Phrase { get; set; }

        public int? WordCount { get; set; }

        public string Context { get; set; }
        public bool? OnlyContext { get; set; }
    }
}
