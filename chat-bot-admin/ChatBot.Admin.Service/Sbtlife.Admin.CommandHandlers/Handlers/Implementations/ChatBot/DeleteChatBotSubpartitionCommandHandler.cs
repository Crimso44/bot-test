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
    internal class DeleteChatBotSubpartitionCommandHandler : CommandHandlerBase<DeleteChatBotSubpartitionCommand>, IDeleteChatBotSubpartitionCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public DeleteChatBotSubpartitionCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IDeleteChatBotSubpartitionCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotPartitionProvider chatBotPartitionProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotPartitionProvider = chatBotPartitionProvider;
        }

        public  ICommandResult Handle(DeleteChatBotSubpartitionCommand command)
        {
            if(!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotSubpartition.Delete, 1);
             _chatBotPartitionProvider.DeletePartition(command.Id);

            return IdResult(MessageConst.ChatBotSubpartitionDeleted, command.Id);
        }
    }
}
