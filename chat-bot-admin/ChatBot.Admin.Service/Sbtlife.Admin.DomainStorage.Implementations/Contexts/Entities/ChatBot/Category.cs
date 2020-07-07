using System;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class Category : EntityInt
    {
        public string Name { get; set; }

        public string Response { get; set; }

        public string SetContext { get; set; }

        public bool? IsDefault { get; set; }
        public bool? IsTest { get; set; }
        public bool? IsDisabled { get; set; }
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
    }
}
