using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class StorePatternCommand : CommandBase
    {
        public PatternDto Pattern { get; set; }
    }
}
