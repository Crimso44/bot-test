using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Response { get; set; }

        public string SetContext { get; set; }
        public int? SetMode { get; set; }

        public bool? IsDefault { get; set; }
        public bool? IsTest { get; set; }
        public bool? IsChanged { get; set; }
        public bool? IsAdded { get; set; }
        public int? ParentId { get; set; }

        public Guid? PartitionId { get; set; }

        public DateTime? ChangedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? PublishedOn { get; set; }

        public Guid? OriginId { get; set; }
        public bool? IsIneligible { get; set; }
        public string RequiredRoster { get; set; }
        public bool? IsDisabled { get; set; }
    }
}
