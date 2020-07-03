using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("Partition")]
    public class Partition
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public string Name { get; set; }
    }
}
