using ChatBot.Admin.CommandHandlers.Model;

namespace ChatBot.Admin.CommandHandlers.Services.Abstractions
{
    public interface ICommandTypeProviderService
    {
        CommandHandlerInfo GetCommandInfoById(string commandId);
    }
}
