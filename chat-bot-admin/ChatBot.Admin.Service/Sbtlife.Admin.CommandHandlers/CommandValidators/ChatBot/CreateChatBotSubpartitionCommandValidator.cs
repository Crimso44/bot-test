using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class CreateChatBotSubpartitionCommandValidator : CommandValidatorBase<CreateChatBotSubpartitionCommand>, ICreateChatBotSubpartitionCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public CreateChatBotSubpartitionCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            RuleFor(cmd => cmd.ParentPartId).NotEmpty().WithMessage(RequiredFieldMessage("Родительская группа"));
            RuleFor(cmd => cmd.Caption).NotEmpty().WithMessage(RequiredFieldMessage("Название"));
            RuleFor(cmd => cmd).Must(CheckPartitionExistsAndNotDeleted).WithMessage(cmd => $"Указанный родительский раздел (\"{cmd.ParentPartId}\") не найден").When(cmd => cmd.Caption != null && cmd.ParentPartId != Guid.Empty);
            RuleFor(cmd => cmd).Must(CheckSubpartitionCaptionExistsAndNotDeleted).WithMessage(cmd => $"В указанном разделе уже существует Подраздел с названием (\"{cmd.Caption}\")").When(cmd => cmd.Caption != null && cmd.ParentPartId != Guid.Empty);
        }


        private  bool CheckPartitionExistsAndNotDeleted(CreateChatBotSubpartitionCommand cmd)
        {
            if (cmd.ParentPartId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.ParentPartId));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.ParentPartId);
        }

        private  bool CheckSubpartitionCaptionExistsAndNotDeleted(CreateChatBotSubpartitionCommand cmd)
        {
            if (cmd.ParentPartId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.ParentPartId));

            if (cmd.Caption == null)
                throw new ArgumentNullException(nameof(cmd.Caption));

            return  _chatBotPartitionProvider.CheckCaptionUnique(cmd.ParentPartId, cmd.Caption);
        }
    }
}
