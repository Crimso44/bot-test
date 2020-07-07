using System;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class DeleteChatBotPartitionCommand : CommandBase
    {
        public Guid Id { get; set; }
    }
}
