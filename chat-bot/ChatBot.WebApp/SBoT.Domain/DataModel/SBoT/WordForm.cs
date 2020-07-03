using System.ComponentModel.DataAnnotations.Schema;


namespace SBoT.Domain.DataModel.SBoT
{
    [Table("WordForm")]
    public class WordForm
    {
        public int Id { get; set; }

        public int WordId { get; set; }

        public string Form { get; set; }
    }
}
