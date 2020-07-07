using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal class DeletePatternCommandValidator : CommandValidatorBase<DeletePatternCommand>, IDeletePatternCommandValidator
    {
        private readonly IChatBotCategoryProvider _chatBotCategoryProvider;
        private PatternDto _pattern;

        public DeletePatternCommandValidator(IChatBotCategoryProvider chatBotCategoryProvider)
        {
            _chatBotCategoryProvider = chatBotCategoryProvider;

            RuleFor(cmd => cmd).Must(PatternExists).WithMessage("Запись не найдена");
        }


        private  bool PatternExists(DeletePatternCommand cmd)
        {
            _pattern =  _chatBotCategoryProvider.GetTestPatternById(cmd.PatternId, null);
            return _pattern != null;
        }


    }
}
