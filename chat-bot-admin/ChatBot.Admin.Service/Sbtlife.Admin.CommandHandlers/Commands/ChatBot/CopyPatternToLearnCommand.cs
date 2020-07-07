using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class CopyPatternToLearnCommand : CommandBase
    {
        public int CategoryId { get; set; }
    }
}