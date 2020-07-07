using System;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class DeleteChatBotSubpartitionCommand : CommandBase
    {
        public Guid Id { get; set; }
    }
}
