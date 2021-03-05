using System;
using System.Collections.Generic;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class CreateChatBotCategoryCommand : CommandBase
    {
        public string Name { get; set; }

        public string Response { get; set; }

        public string SetContext { get; set; }
        public int? SetMode { get; set; }

        public Guid? PartitionId { get; set; }
        public bool? IsIneligible { get; set; }
        public bool? IsDisabled { get; set; }
        public string RequiredRoster { get; set; }

        public List<PatternDto> Patterns { get; set; }
    }
}
