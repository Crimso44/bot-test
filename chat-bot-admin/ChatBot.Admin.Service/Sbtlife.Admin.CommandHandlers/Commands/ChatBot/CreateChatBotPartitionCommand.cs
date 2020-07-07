using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class CreateChatBotPartitionCommand : CommandBase
    {
        public string Caption { get; set; }
    }
}
