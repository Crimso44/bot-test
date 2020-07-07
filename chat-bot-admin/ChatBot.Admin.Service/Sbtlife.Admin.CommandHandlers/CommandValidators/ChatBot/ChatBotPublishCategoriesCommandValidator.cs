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
    internal class ChatBotPublishCategoriesCommandValidator : CommandValidatorBase<ChatBotPublishCategoriesCommand>, IChatBotPublishCategoriesCommandValidator
    {
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;
        private readonly IChatBotCategoryProvider _chatBotCategoryProvider;

        public ChatBotPublishCategoriesCommandValidator(IChatBotPartitionProvider chatBotPartitionProvider, IChatBotCategoryProvider chatBotCategoryProvider)
        {
            _chatBotPartitionProvider = chatBotPartitionProvider;
            _chatBotCategoryProvider = chatBotCategoryProvider;

            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.PartitionId}\") не найден").When(cmd => cmd.PartitionId.HasValue);
            RuleFor(cmd => cmd).Must(CheckSubPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.SubPartId}\") не найден").When(cmd => cmd.SubPartId.HasValue);
            RuleFor(cmd => cmd).Must(CheckAnyConfig).WithMessage("Нет настроек в редактируемой области");
        }


        private  bool CheckPartitionIdExistsAndNotDeleted(ChatBotPublishCategoriesCommand cmd)
        {
            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.PartitionId.Value);
        }

        private  bool CheckSubPartitionIdExistsAndNotDeleted(ChatBotPublishCategoriesCommand cmd)
        {
            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.SubPartId.Value);
        }

        private  bool CheckAnyConfig(ChatBotPublishCategoriesCommand cmd)
        {
            return  _chatBotCategoryProvider.CheckAnyEditCategories();
        }
    }
}
