using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("PatternWordRel")]
    public class PatternWordRel
    {
        public int PatternId { get; set; }

        public int WordId { get; set; }
    }
}
