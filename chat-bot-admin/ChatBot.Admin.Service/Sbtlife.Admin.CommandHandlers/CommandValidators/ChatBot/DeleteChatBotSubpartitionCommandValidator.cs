using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class DeleteChatBotSubpartitionCommandValidator : CommandValidatorBase<DeleteChatBotSubpartitionCommand>, IDeleteChatBotSubpartitionCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public DeleteChatBotSubpartitionCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            RuleFor(cmd => cmd.Id).NotEqual(Guid.Empty).WithMessage("Не указан идентификатор подраздела");
            RuleFor(cmd => cmd).Must(CheckSubpartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Подраздел (Id=\"{cmd.Id}\") не найден").When(cmd => cmd.Id != Guid.Empty);
            RuleFor(cmd => cmd).Must(CheckSubpartitionHasNoLinks).WithMessage(cmd => $"Подраздел (Id=\"{cmd.Id}\") содержит категории").When(cmd => cmd.Id != Guid.Empty);
        }

        private  bool CheckSubpartitionIdExistsAndNotDeleted(DeleteChatBotSubpartitionCommand cmd)
        {
            if(cmd.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.Id);
        }

        private  bool CheckSubpartitionHasNoLinks(DeleteChatBotSubpartitionCommand cmd)
        {
            if (cmd.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(cmd.Id));

            return !  _chatBotPartitionProvider.CheckHasLinks(cmd.Id);
        }
    }
}
