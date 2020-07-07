using System;
using System.Collections.Generic;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot
{
    public class CategoryOptionalDto
    {
        public int Id { get; set; }

        public Optional<string> Name { get; set; }

        public Optional<string> Response { get; set; }

        public Optional<string> SetContext { get; set; }

        public Optional<Guid?> PartitionId { get; set; }

        public List<PatternDto> Patterns { get; set; }
        public bool? IsChangedPatterns { get; set; }

        public bool? IsChanged { get; set; }
        public bool? IsTest { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsAdded { get; set; }
        public DateTime? ChangedOn { get; set; }
        public string ChangedBy { get; set; }
        public Guid? OriginId { get; set; }
        public bool? IsIneligible { get; set; }
        public string RequiredRoster { get; set; }
    }
}
