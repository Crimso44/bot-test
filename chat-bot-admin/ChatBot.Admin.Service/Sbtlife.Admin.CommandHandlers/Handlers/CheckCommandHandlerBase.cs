using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers
{
    internal abstract class CheckCommandHandlerBase<TCommand> : CommandHandlerCommonBase<TCommand>
        where TCommand : CommandBase
    {

        protected CheckCommandHandlerBase(ICommandProvider commandProvider,
            IValidator<TCommand> validator)
            : base(commandProvider, validator)
        {
        }

        public  ICommandResult Handle(TCommand command)
        {
             ValidateCommandIsNotAlreadyProcessed(command);
             CheckCommandFields(command);

            return Ok(MessageConst.CheckSuccess);
        }
    }
}
