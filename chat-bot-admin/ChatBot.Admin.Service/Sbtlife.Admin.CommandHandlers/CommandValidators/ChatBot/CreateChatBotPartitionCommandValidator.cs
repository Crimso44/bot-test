using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class CreateChatBotPartitionCommandValidator : CommandValidatorBase<CreateChatBotPartitionCommand>, ICreateChatBotPartitionCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public CreateChatBotPartitionCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            RuleFor(cmd => cmd.Caption).NotEmpty().WithMessage(RequiredFieldMessage("Название"));
            RuleFor(cmd => cmd).Must(CheckPartitionCaptionExistsAndNotDeleted).WithMessage(cmd => $"Уже существует Раздел с названием (\"{cmd.Caption}\")").When(cmd => cmd.Caption != null);
        }

        private  bool CheckPartitionCaptionExistsAndNotDeleted(CreateChatBotPartitionCommand cmd)
        {
            if(cmd.Caption == null)
                throw new ArgumentNullException(nameof(cmd.Caption));

            return  _chatBotPartitionProvider.CheckCaptionUnique(cmd.Caption);
        }
    }
}
