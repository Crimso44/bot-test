using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Model;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.Common.Model;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers
{
    internal abstract class CommandHandlerCommonBase<TCommand>
        where TCommand : CommandBase
    {
        private readonly ICommandProvider _commandProvider;
        private readonly IValidator<TCommand> _validator;

        public CommandHandlerCommonBase(ICommandProvider commandProvider,
            IValidator<TCommand> validator)
        {
            _commandProvider = commandProvider;
            _validator = validator;
        }

        protected ICommandResult IdResult(string text, Guid id)
        {
            return new CommandResult
            {
                Error = false,
                Payload = new { id },
                Text = text
            };
        }

        protected ICommandResult IdIntResult(string text, int id)
        {
            return new CommandResult
            {
                Error = false,
                Payload = new { id },
                Text = text
            };
        }

        protected ICommandResult Ok(string text = null, object payload = null)
        {
            return new CommandResult
            {
                Error = false,
                Payload = payload,
                Text = text
            };
        }

        protected  void CheckCommandFields(TCommand command)
        {
            var result =  _validator.Validate(command);

            if (!result.IsValid)
                throw new BusinessLogicException(MessageConst.CheckFail, BuildErrorPayload(result.Errors));
        }

        protected  void ValidateCommandIsNotAlreadyProcessed(ICommand command)
        {
            if ( _commandProvider.IsDocumentAlreadyProcessed(command.GetId()))
                throw new BusinessLogicException(MessageConst.CommandAlreadyProcessed);
        }

        private CommonError[] BuildErrorPayload(IEnumerable<ValidationFailure> errors)
        {
            return errors.Select(e =>
                new CommonError
                {
                    Severity = e.Severity.ToString(),
                    Message = e.ErrorMessage
                })
                .ToArray();
        }
    }
}
