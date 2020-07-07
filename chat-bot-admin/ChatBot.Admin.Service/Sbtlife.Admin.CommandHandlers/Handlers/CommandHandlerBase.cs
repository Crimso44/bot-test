using System;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;
using ChatBot.Admin.Common.Exceptions;

namespace ChatBot.Admin.CommandHandlers.Handlers
{
    internal abstract class CommandHandlerBase<TCommand> : CommandHandlerCommonBase<TCommand>
        where TCommand : CommandBase
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ICommandProvider _commandProvider;
        private readonly IJsonSerializerService _jsonSerializerService;

        protected CommandHandlerBase(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IValidator<TCommand> validator,
            IJsonSerializerService jsonSerializerService)
            : base(commandProvider, validator)
        {
            _commandFactory = commandFactory;
            _commandProvider = commandProvider;
            _jsonSerializerService = jsonSerializerService;
        }

        protected void CheckAndStoreCommand(TCommand command, Guid commandTypeId, int commandVersion)
        {
            ValidateCommandIsNotAlreadyProcessed(command);
            CheckCommandFields(command);
            StoreCommand(command, commandTypeId, commandVersion);
        }

        private  void StoreCommand(TCommand command, Guid commandTypeId, int commandVersion)
        {
            var commandDoc = _commandFactory.GetCommand(command.GetId(), commandTypeId, commandVersion);
            commandDoc.Payload = _jsonSerializerService.GetJsonString(command);
             _commandProvider.AddCommand(commandDoc);
        }
    }
}
