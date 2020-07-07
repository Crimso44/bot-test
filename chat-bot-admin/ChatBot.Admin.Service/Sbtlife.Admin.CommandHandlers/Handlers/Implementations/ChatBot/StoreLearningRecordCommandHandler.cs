using System;
using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class StoreLearningRecordCommandHandler : CommandHandlerBase<StoreLearningRecordCommand>, IStoreLearningRecordCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotLearningProvider _chatBotLearningsProvider;

        public StoreLearningRecordCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IStoreLearningRecordCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotLearningProvider chatBotLearningsProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotLearningsProvider = chatBotLearningsProvider;
        }

        public  ICommandResult Handle(StoreLearningRecordCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotLearning.Store, 1);

            if (command.Learning.Id.HasValue)
                 _chatBotLearningsProvider.ModifyLearning(command.Learning);
            else
                 _chatBotLearningsProvider.AddLearning(command.Learning);

            return Ok(MessageConst.ChatBotLearningStored);
        }
    }
}
