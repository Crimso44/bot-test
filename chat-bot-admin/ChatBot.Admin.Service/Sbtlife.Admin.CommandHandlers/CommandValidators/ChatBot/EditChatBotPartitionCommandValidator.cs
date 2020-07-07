using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class EditChatBotPartitionCommandValidator : CommandValidatorBase<EditChatBotPartitionCommand>, IEditChatBotPartitionCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public EditChatBotPartitionCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Не указан идентификатор раздела");
            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.Id}\") не найден").When(cmd => cmd.Id != Guid.Empty);
            RuleFor(cmd => cmd.Caption.Value).NotEmpty().WithMessage(RequiredFieldMessage("Название")).When(cmd => cmd.Caption != null);
        }

        private  bool CheckPartitionIdExistsAndNotDeleted(EditChatBotPartitionCommand cmd)
        {
            if (cmd.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.Id);
        }
    }
}
