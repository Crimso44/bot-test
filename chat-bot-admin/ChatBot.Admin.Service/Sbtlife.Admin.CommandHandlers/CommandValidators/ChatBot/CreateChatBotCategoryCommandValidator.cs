using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal class CreateChatBotCategoryCommandValidator : CommandValidatorBase<CreateChatBotCategoryCommand>, ICreateChatBotCategoryCommandValidator
    {
        private readonly IChatBotCategoryProvider _categoryProvider;
        private readonly IChatBotPartitionProvider _chatBotPartitionProvider;
        private readonly IWordService _wordService;

        public CreateChatBotCategoryCommandValidator(IChatBotCategoryProvider categoryProvider, IChatBotPartitionProvider chatBotPartitionProvider, IWordService wordService)
        {
            _categoryProvider = categoryProvider;
            _chatBotPartitionProvider = chatBotPartitionProvider;
            _wordService = wordService;

            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage(RequiredFieldMessage("Название"));
            RuleFor(cmd => cmd.Response).NotEmpty().WithMessage(RequiredFieldMessage("Ответ"));
            RuleFor(cmd => cmd).Must(ValidateResponse).When(cmd => cmd.Response != null);
            RuleFor(cmd => cmd).Must(CheckPartitionIdExistsAndNotDeleted).WithMessage(cmd => $"Раздел (Id=\"{cmd.PartitionId}\") не найден").When(cmd => cmd.PartitionId != null);
            RuleFor(cmd => cmd.Patterns).SetCollectionValidator(new ChatBotPatternValidator(_wordService));
            RuleFor(cmd => cmd).Must(CheckCaptionUnique).WithMessage(cmd => $"Уже существует Категория с названием (\"{cmd.Name}\")");
        }


        private bool ValidateResponse(CreateChatBotCategoryCommand cmd)
        {
            var res =  _categoryProvider.ValidateResponse(cmd.Response);
            if (!string.IsNullOrEmpty(res)) throw new BusinessLogicException(res);
            return true;
        }



        private  bool CheckCaptionUnique(CreateChatBotCategoryCommand cmd)
        {
            if (cmd.Name == null)
                throw new ArgumentNullException(nameof(cmd.Name));

            return  _categoryProvider.CheckCaptionUnique(cmd.Name);
        }

        private  bool CheckPartitionIdExistsAndNotDeleted(CreateChatBotCategoryCommand cmd)
        {
            if(cmd.PartitionId == null)
                throw new ArgumentNullException(nameof(cmd.PartitionId));

            return  _chatBotPartitionProvider.CheckExistsAndNotDeleted(cmd.PartitionId.Value);
        }

    }
}
