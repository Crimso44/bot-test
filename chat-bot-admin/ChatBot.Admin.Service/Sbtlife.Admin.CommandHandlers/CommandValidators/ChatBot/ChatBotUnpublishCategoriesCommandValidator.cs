using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal class ChatBotUnpublishCategoriesCommandValidator : CommandValidatorBase<ChatBotUnpublishCategoriesCommand>, IChatBotUnpublishCategoriesCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;

        public ChatBotUnpublishCategoriesCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;

            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.PartitionId}\") не найден").When(cmd => cmd.PartitionId.HasValue);
            RuleFor(cmd => cmd).Must(CheckSubPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.SubPartId}\") не найден").When(cmd => cmd.SubPartId.HasValue);
        }


        private  bool CheckPartitionIdExistsAndNotDeleted(ChatBotUnpublishCategoriesCommand cmd)
        {
            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.PartitionId.Value);
        }

        private  bool CheckSubPartitionIdExistsAndNotDeleted(ChatBotUnpublishCategoriesCommand cmd)
        {
            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.SubPartId.Value);
        }
    }
}
