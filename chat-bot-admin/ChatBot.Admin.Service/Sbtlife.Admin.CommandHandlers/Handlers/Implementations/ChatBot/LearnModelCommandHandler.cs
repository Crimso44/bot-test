using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class LearnModelCommandHandler : CommandHandlerBase<LearnModelCommand>, ILearnModelCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotLearningProvider _chatBotLearningProvider;

        public LearnModelCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            ILearnModelCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotLearningProvider chatBotLearningProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotLearningProvider = chatBotLearningProvider;
        }

        public  ICommandResult Handle(LearnModelCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotSetting.Save, 1);

             _chatBotLearningProvider.RunLearningCommand(command.ModelCommand);

            return Ok(MessageConst.ChatBotModelLearningStarted);
        }
    }

}
