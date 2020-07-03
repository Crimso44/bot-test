using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("Word")]
    public class Word
    {
        public int Id { get; set; }

        public string WordName { get; set; }

        public int? WordTypeId { get; set; }
    }
}
