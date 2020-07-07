using System;
using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class DeleteChatBotCategoryCommandHandler : CommandHandlerBase<DeleteChatBotCategoryCommand>, IDeleteChatBotCategoryCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryProvider _categoryProvider;

        public DeleteChatBotCategoryCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IDeleteChatBotCategoryCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotCategoryProvider categoryProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _categoryProvider = categoryProvider;
        }

        public  ICommandResult Handle(DeleteChatBotCategoryCommand command)
        {
            if(!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.Delete, 1);
             _categoryProvider.DeleteCategory(command.Id);

            return IdIntResult(MessageConst.CategoryDeleted, command.Id);
        }
    }
}
