using System;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class DeleteChatBotCategoryCommand : CommandBase
    {
        public int Id { get; set; }
    }
}
