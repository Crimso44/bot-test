using System;
using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class CreateChatBotCategoryCommandHandler : CommandHandlerBase<CreateChatBotCategoryCommand>, ICreateChatBotCategoryCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryFactory _categoryFactory;
        private readonly IChatBotCategoryProvider _categoryProvider;

        public CreateChatBotCategoryCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            ICreateChatBotCategoryCommandValidator validator,
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

        public  ICommandResult Handle(CreateChatBotCategoryCommand command)
        {
            if(!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.Create, 1);

            var category = _categoryFactory.GetCategory();
            category.Name = command.Name;
            category.Response = command.Response;
            category.SetContext = command.SetContext;
            category.SetMode = command.SetMode.HasValue ? command.SetMode.ToString() : null;
            category.PartitionId = command.PartitionId;
            category.Patterns = command.Patterns;
            category.IsIneligible = command.IsIneligible;
            category.IsDisabled = command.IsDisabled;
            category.RequiredRoster = command.RequiredRoster;

             _categoryProvider.AddCategory(category);

            return IdIntResult(MessageConst.CategoryCreated, category.Id);
        }
    }
}
