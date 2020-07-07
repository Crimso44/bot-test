using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Model
{
    internal class CommandResult : ICommandResult
    {
        public string Text { get; set; }
        public bool Error { get; set; }
        public object Payload { get; set; }
    }
}
