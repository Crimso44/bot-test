using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal class StorePatternCommandValidator : CommandValidatorBase<StorePatternCommand>, IStorePatternCommandValidator
    {
        private readonly IChatBotCategoryProvider _chatBotCategoryProvider;
        private readonly IWordService _wordService;
        private PatternDto _pattern;

        public StorePatternCommandValidator(IChatBotCategoryProvider chatBotCategoryProvider, IWordService wordService)
        {
            _chatBotCategoryProvider = chatBotCategoryProvider;
            _wordService = wordService;

            RuleFor(cmd => cmd.Pattern).NotNull().WithMessage("Не указан паттерн");
            RuleFor(cmd => cmd.Pattern.CategoryId).NotEmpty().WithMessage("Не указана категория");
            RuleFor(cmd => cmd).Must(PatternExists).When(cmd => (cmd.Pattern.Id ?? 0) != 0).WithMessage("Запись не найдена");
            RuleFor(cmd => cmd.Pattern).SetValidator(new ChatBotPatternValidator(_wordService));
        }


        private  bool PatternExists(StorePatternCommand cmd)
        {
            _pattern =  _chatBotCategoryProvider.GetTestPatternById(cmd.Pattern.Id.Value, cmd.Pattern.CategoryId);
            return _pattern != null;
        }

    }
}
