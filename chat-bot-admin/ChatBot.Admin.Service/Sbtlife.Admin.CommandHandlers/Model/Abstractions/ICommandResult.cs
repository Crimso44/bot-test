
namespace ChatBot.Admin.CommandHandlers.Model.Abstractions
{
    public interface ICommandResult
    {
        string Text { get; }
        bool Error { get; }
        object Payload { get; }
    }
}
