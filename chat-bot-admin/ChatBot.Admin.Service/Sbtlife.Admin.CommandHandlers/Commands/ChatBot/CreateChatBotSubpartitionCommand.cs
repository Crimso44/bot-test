using System;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class CreateChatBotSubpartitionCommand : CommandBase
    {
        public Guid ParentPartId { get; set; }
        public string Caption { get; set; }
    }
}
