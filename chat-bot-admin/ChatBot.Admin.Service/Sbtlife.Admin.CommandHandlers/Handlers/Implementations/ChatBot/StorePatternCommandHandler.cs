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
    internal class StorePatternCommandHandler : CommandHandlerBase<StorePatternCommand>, IStorePatternCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryProvider _categoryProvider;

        public StorePatternCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IStorePatternCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotCategoryProvider categoryProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _categoryProvider = categoryProvider;
        }

        public  ICommandResult Handle(StorePatternCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.StorePattern, 1);

             _categoryProvider.StorePattern(command.Pattern);

            return IdIntResult(MessageConst.PatternStored, command.Pattern.Id ?? 0);
        }
    }
}
