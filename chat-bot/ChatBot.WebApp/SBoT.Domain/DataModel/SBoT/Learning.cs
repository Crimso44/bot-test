using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("Learning")]
    public class Learning
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PartitionId { get; set; }
        public string Tokens { get; set; }
    }
}
