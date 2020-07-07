using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Nest;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal class StoreLearnModelReportCommandValidator : CommandValidatorBase<StoreLearnModelReportCommand>, IStoreLearnModelReportCommandValidator
    {
        private readonly IChatBotLearningProvider _chatBotLearningProvider;

        public StoreLearnModelReportCommandValidator(IChatBotLearningProvider chatBotLearningProvider)
        {
            _chatBotLearningProvider = chatBotLearningProvider;

            RuleFor(cmd => cmd).Must(LearnModelExists).WithMessage("Запись не найдена");
            RuleFor(cmd => cmd.Report).NotNull();
            //RuleFor(cmd => cmd).Must(QuestionIsUnique).WithMessage(cmd => $"{cmd.ErrorMessage}");
        }


        private  bool LearnModelExists(StoreLearnModelReportCommand cmd)
        {
            var learning =  _chatBotLearningProvider.GetModelLearning(cmd.ModelId);
            return learning != null;
        }

    }
}
