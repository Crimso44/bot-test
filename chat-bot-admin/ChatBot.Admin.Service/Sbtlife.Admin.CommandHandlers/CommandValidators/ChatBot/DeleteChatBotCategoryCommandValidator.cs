using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class DeleteChatBotCategoryCommandValidator : CommandValidatorBase<DeleteChatBotCategoryCommand>, IDeleteChatBotCategoryCommandValidator
    {
        private readonly IChatBotCategoryProvider _categoryProvider;

        public DeleteChatBotCategoryCommandValidator(IChatBotCategoryProvider categoryProvider)
        {
            _categoryProvider = categoryProvider;
            RuleFor(cmd => cmd.Id).NotEqual(0).WithMessage("Не указан идентификатор ссылки");
            RuleFor(cmd => cmd).Must(CheckCategoryIdExistsAndNotDeleted).WithMessage(cmd => $"Ссылка (Id=\"{cmd.Id}\") не найдена").When(cmd => cmd.Id != 0);
            RuleFor(cmd => cmd).Must(CheckCategoryIsEditable).WithMessage(cmd => $"Категория опубликована и не может быть удалена");
            RuleFor(cmd => cmd).Must(ValidateDelete);
        }

        private  bool CheckCategoryIdExistsAndNotDeleted(DeleteChatBotCategoryCommand cmd)
        {
            if(cmd.Id == 0)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _categoryProvider.CheckExistsAndNotDeleted(cmd.Id);
        }

        private  bool CheckCategoryIsEditable(DeleteChatBotCategoryCommand cmd)
        {
            if (cmd.Id == 0)
                throw new ArgumentNullException(nameof(cmd.Id));

            return  _categoryProvider.CheckCategoryIsEditable(cmd.Id);
        }

        private  bool ValidateDelete(DeleteChatBotCategoryCommand cmd)
        {
            var res =  _categoryProvider.ValidateDelete(cmd.Id);
            if (!string.IsNullOrEmpty(res)) throw new BusinessLogicException(res);
            return true;
        }



    }
}
