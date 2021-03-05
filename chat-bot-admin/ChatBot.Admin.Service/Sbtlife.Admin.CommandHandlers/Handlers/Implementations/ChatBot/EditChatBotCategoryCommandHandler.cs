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
    internal class EditChatBotCategoryCommandHandler : CommandHandlerBase<EditChatBotCategoryCommand>, IEditChatBotCategoryCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryFactory _categoryFactory;
        private readonly IChatBotCategoryProvider _categoryProvider;

        public EditChatBotCategoryCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IEditChatBotCategoryCommandValidator validator,
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

        public  ICommandResult Handle(EditChatBotCategoryCommand command)
        {
            if(!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.Edit, 1);

            var category = _categoryFactory.GetEditableCategory(command.Id);
            category.Name = command.Name;
            category.Response = command.Response;
            category.SetContext = command.SetContext;
            category.SetMode = command.SetMode;
            category.PartitionId = command.PartitionId;
            category.Patterns = command.Patterns;
            category.IsChangedPatterns = command.IsChangedPatterns;
            category.IsIneligible = command.IsIneligible;
            category.IsDisabled = command.IsDisabled;
            category.RequiredRoster = command.RequiredRoster;

             _categoryProvider.ModifyCategory(category);

            return IdIntResult(MessageConst.CategoryEdited, category.Id);
        }
    }
}
