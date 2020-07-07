using System;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class EditChatBotSubpartitionCommand : CommandBase
    {
        public Guid Id { get; set; }
        public Optional<Guid?> ParentPartId { get; set; }
        public Optional<string> Caption { get; set; }
    }
}
