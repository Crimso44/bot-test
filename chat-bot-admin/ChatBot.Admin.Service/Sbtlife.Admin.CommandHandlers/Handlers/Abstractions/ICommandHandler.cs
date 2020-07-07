using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Handlers.Abstractions
{
    public interface ICommandHandler<in TCommand>
    {
        ICommandResult Handle(TCommand command);
    }
}
