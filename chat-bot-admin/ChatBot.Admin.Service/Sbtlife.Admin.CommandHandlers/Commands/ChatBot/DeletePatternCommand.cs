using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class DeletePatternCommand : CommandBase
    {
        public int PatternId { get; set; }
    }
}
