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
    internal class DeletePatternCommandHandler : CommandHandlerBase<DeletePatternCommand>, IDeletePatternCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryProvider _chatBotCategorysProvider;

        public DeletePatternCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IDeletePatternCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotCategoryProvider chatBotCategorysProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotCategorysProvider = chatBotCategorysProvider;
        }

        public  ICommandResult Handle(DeletePatternCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.DeletePattern, 1);

             _chatBotCategorysProvider.DeletePattern(command.PatternId);

            return Ok(MessageConst.PatternDeleted);
        }
    }
}
