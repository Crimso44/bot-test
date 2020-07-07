using System;
using System.Collections.Generic;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class EditChatBotCategoryCommand : CommandBase
    {
        public int Id { get; set; }

        public Optional<string> Name { get; set; }

        public Optional<string> Response { get; set; }

        public Optional<string> SetContext { get; set; }

        public Optional<Guid?> PartitionId { get; set; }

        public List<PatternDto> Patterns { get; set; }
        public bool? IsChangedPatterns { get; set; }
        public bool? IsIneligible { get; set; }
        public bool? IsDisabled { get; set; }
        public string RequiredRoster { get; set; }

    }
}
