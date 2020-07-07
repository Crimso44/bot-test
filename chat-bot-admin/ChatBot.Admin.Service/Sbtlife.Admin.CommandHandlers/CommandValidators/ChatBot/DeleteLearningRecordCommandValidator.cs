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
    internal class DeleteLearningRecordCommandValidator : CommandValidatorBase<DeleteLearningRecordCommand>, IDeleteLearningRecordCommandValidator
    {
        private readonly IChatBotLearningProvider _chatBotLearningProvider;
        private LearningDto _learning;

        public DeleteLearningRecordCommandValidator(IChatBotLearningProvider chatBotLearningProvider)
        {
            _chatBotLearningProvider = chatBotLearningProvider;

            RuleFor(cmd => cmd).Must(LearningExists).WithMessage("Запись не найдена");
        }


        private  bool LearningExists(DeleteLearningRecordCommand cmd)
        {
            _learning =  _chatBotLearningProvider.GetByIdAndQuestion(cmd.LearningId, null);
            return _learning != null;
        }


    }
}
