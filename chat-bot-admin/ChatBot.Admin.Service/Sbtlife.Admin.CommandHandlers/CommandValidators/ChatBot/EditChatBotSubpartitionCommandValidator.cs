using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class EditChatBotSubpartitionCommandValidator : CommandValidatorBase<EditChatBotSubpartitionCommand>, IEditChatBotSubpartitionCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public EditChatBotSubpartitionCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Не указан идентификатор подраздела");
            RuleFor(cmd => cmd).Must(CheckSubpartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Подраздел (Id=\"{cmd.Id}\") не найден").When(cmd => cmd.Id != Guid.Empty);
            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.ParentPartId.Value}\") не найден").When(cmd => cmd.ParentPartId != null);
            RuleFor(cmd => cmd.Caption.Value).NotEmpty().WithMessage(RequiredFieldMessage("Название")).When(cmd => cmd.Caption != null);
        }

        private  bool CheckSubpartitionIdExistsAndNotDeleted(EditChatBotSubpartitionCommand cmd)
        {
            if (cmd.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.Id);
        }

        private  bool CheckPartitionIdExistsAndNotDeleted(EditChatBotSubpartitionCommand cmd)
        {
            if (cmd.ParentPartId == null)
                throw new ArgumentNullException(nameof(cmd.ParentPartId));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.ParentPartId.Value.Value);
        }
    }
}
