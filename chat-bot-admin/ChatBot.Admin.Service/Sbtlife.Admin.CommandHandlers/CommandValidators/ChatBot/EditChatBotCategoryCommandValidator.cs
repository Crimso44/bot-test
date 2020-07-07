using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class EditChatBotCategoryCommandValidator : CommandValidatorBase<EditChatBotCategoryCommand>, IEditChatBotCategoryCommandValidator
    {
        private readonly IChatBotCategoryProvider _categoryProvider;
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;
        private readonly IWordService _wordService;

        public EditChatBotCategoryCommandValidator(IChatBotCategoryProvider categoryProvider, IChatBotPartitionProvider chatBotPartitionProvider, IWordService wordService)
        {
            _categoryProvider = categoryProvider;
            _chatBotPartitionProvider = chatBotPartitionProvider;
            _wordService = wordService;

            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Не указан идентификатор ссылки");
            RuleFor(cmd => cmd).Must(CheckCategoryIdExistsAndNotDeleted).WithMessage(cmd => $"Ссылка (Id=\"{cmd.Id}\") не найдена").When(cmd => cmd.Id != 0);
            RuleFor(cmd => cmd.Name.Value).NotEmpty().WithMessage(RequiredFieldMessage("Название")).When(cmd => cmd.Name != null);
            RuleFor(cmd => cmd).Must(CheckCategoryIsEditable).WithMessage(cmd => $"Категория опубликована и не может быть изменена");
            RuleFor(cmd => cmd.Response.Value).NotEmpty().WithMessage(RequiredFieldMessage("Ответ")).When(cmd => cmd.Response != null);
            RuleFor(cmd => cmd).Must(ValidateResponse).When(cmd => cmd.Response != null);
            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.PartitionId}\") не найден").When(cmd => cmd.PartitionId?.Value == null);
            RuleFor(cmd => cmd.Patterns).SetCollectionValidator(new ChatBotPatternValidator(_wordService)).When(cmd => cmd.IsChangedPatterns ?? false);
            RuleFor(cmd => cmd).Must(CheckCaptionUnique).WithMessage(cmd => $"Уже существует Категория с названием (\"{cmd.Name}\")").When(cmd => cmd.Name != null);
        }

        private  bool CheckCaptionUnique(EditChatBotCategoryCommand cmd)
        {
            if (cmd.Name == null)
                throw new ArgumentNullException(nameof(cmd.Name));

            return  _categoryProvider.CheckCaptionUnique(cmd.Name.Value);
        }

        private  bool ValidateResponse(EditChatBotCategoryCommand cmd)
        {
            var res =  _categoryProvider.ValidateResponse(cmd.Response.Value);
            if (!string.IsNullOrEmpty(res)) throw new BusinessLogicException(res);
            return true;
        }

        private  bool CheckCategoryIsEditable(EditChatBotCategoryCommand cmd)
        {
            if (cmd.Id == 0)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _categoryProvider.CheckCategoryIsEditable(cmd.Id);
        }

        private  bool CheckCategoryIdExistsAndNotDeleted(EditChatBotCategoryCommand cmd)
        {
            if (cmd.Id == 0)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _categoryProvider.CheckExistsAndNotDeleted(cmd.Id);
        }

        private  bool CheckPartitionIdExistsAndNotDeleted(EditChatBotCategoryCommand cmd)
        {
            if (cmd.PartitionId == null) return true;

            if (cmd.PartitionId?.Value == null)
                throw new ArgumentNullException(nameof(cmd.PartitionId));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.PartitionId.Value.Value);
        }


    }
}
