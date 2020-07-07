using System;
using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class ChatBotUnpublishCategoriesCommandHandler : CommandHandlerBase<ChatBotUnpublishCategoriesCommand>, IChatBotUnpublishCategoriesCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryFactory _categoryFactory;
        private readonly IChatBotCategoryProvider _categoryProvider;

        public ChatBotUnpublishCategoriesCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IChatBotUnpublishCategoriesCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotCategoryFactory categoryFactory,
            IChatBotCategoryProvider categoryProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _categoryFactory = categoryFactory;
            _categoryProvider = categoryProvider;
        }

        public  ICommandResult Handle(ChatBotUnpublishCategoriesCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.Create, 1);

             _categoryProvider.UnpublishCategories(command.PartitionId, command.SubPartId);

            return Ok(MessageConst.CategoryUnpublished);
        }
    }
}
