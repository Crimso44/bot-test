using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class DeleteChatBotPartitionCommandValidator : CommandValidatorBase<DeleteChatBotPartitionCommand>, IDeleteChatBotPartitionCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public DeleteChatBotPartitionCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Не указан идентификатор группы");
            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.Id}\") не найден").When(cmd => cmd.Id != Guid.Empty);
            RuleFor(cmd => cmd).Must(CheckPartitionHasNoSubpartitions).WithMessage(cmd => $"Раздел (Id=\"{cmd.Id}\") содержит подразделы").When(cmd => cmd.Id != Guid.Empty);
        }

        private  bool CheckPartitionIdExistsAndNotDeleted(DeleteChatBotPartitionCommand cmd)
        {
            if(cmd.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.Id);
        }

        private  bool CheckPartitionHasNoSubpartitions(DeleteChatBotPartitionCommand cmd)
        {
            if (cmd.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(cmd.Id));

            return !  _chatBotPartitionProvider.CheckHasSubpartitions(cmd.Id);
        }
    }
}
