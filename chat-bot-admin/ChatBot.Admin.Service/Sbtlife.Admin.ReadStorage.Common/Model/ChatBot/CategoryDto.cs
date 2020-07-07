using System;
using System.Collections.Generic;
using System.Text;

namespace Sbtlife.Admin.ReadStorage.Common.Model.ChatBot
{
    public class CategoryDto : DtoIntBase
    {
        public string Name { get; set; }

        public string Response { get; set; }

        public string SetContext { get; set; }

        public bool IsDefault { get; set; }
        public bool? IsTest { get; set; }
        public bool? IsChanged { get; set; }
        public bool? IsAdded { get; set; }
        public int? ParentId { get; set; }

        public Guid? PartitionId { get; set; }
        public PartitionDto Partition { get; set; }
        public PartitionDto UpperPartition { get; set; }

        public DateTime? ChangedOn { get; set; }
        public string ChangedBy { get; set; }
        public string ChangedByName { get; set; }
        public DateTime? PublishedOn { get; set; }

        public List<PatternDto> Patterns { get; set; }
    }
}
