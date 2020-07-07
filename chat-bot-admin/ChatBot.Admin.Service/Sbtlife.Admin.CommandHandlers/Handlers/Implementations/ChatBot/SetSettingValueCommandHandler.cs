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
    internal class SetSettingValueCommandHandler : CommandHandlerBase<SetSettingValueCommand>, ISetSettingValueCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotSettingsProvider _chatBotSettingsProvider;

        public SetSettingValueCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            ISetSettingValueCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotSettingsProvider chatBotSettingsProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotSettingsProvider = chatBotSettingsProvider;
        }

        public  ICommandResult Handle(SetSettingValueCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotSetting.Save, 1);

             _chatBotSettingsProvider.UpdateSettingValue(command.Name, command.Value);

            return Ok(MessageConst.ChatBotSettingUpdated);
        }
    }
}
