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
    internal class EditChatBotSubpartitionCommandHandler : CommandHandlerBase<EditChatBotSubpartitionCommand>, IEditChatBotSubpartitionCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotPartitionFactory _chatBotPartitionFactory;
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public EditChatBotSubpartitionCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IEditChatBotSubpartitionCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotPartitionFactory chatBotPartitionFactory,
            IChatBotPartitionProvider chatBotPartitionProvider)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotPartitionFactory = chatBotPartitionFactory;
            _chatBotPartitionProvider = chatBotPartitionProvider;
        }

        public  ICommandResult Handle(EditChatBotSubpartitionCommand command)
        {
            if(!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotSubpartition.Edit, 1);

            var partition = _chatBotPartitionFactory.GetEditablePartition(command.Id);
            partition.Title = command.Caption;
            partition.ParentId = command.ParentPartId;

             _chatBotPartitionProvider.ModifyPartition(partition);

            return IdResult(MessageConst.ChatBotSubpartitionEdited, partition.Id);
        }
    }
}
