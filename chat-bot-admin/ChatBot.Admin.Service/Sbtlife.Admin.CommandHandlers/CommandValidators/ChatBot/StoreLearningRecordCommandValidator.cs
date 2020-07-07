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
    internal class StoreLearningRecordCommandValidator : CommandValidatorBase<StoreLearningRecordCommand>, IStoreLearningRecordCommandValidator
    {
        private readonly IChatBotLearningProvider _chatBotLearningProvider;
        private readonly IChatBotCategoryProvider _chatBotCategoryProvider;
        private LearningDto _learning;

        public StoreLearningRecordCommandValidator(IChatBotLearningProvider chatBotLearningProvider, IChatBotCategoryProvider chatBotCategoryProvider)
        {
            _chatBotLearningProvider = chatBotLearningProvider;
            _chatBotCategoryProvider = chatBotCategoryProvider;

            RuleFor(cmd => cmd).Must(LearningExists).When(cmd => cmd.Learning.Id.HasValue).WithMessage("Запись не найдена");
            RuleFor(cmd => cmd.Learning.Question).NotNull().NotEmpty();
            //RuleFor(cmd => cmd).Must(QuestionIsUnique).WithMessage(cmd => $"{cmd.ErrorMessage}");
        }


        private  bool LearningExists(StoreLearningRecordCommand cmd)
        {
            _learning =  _chatBotLearningProvider.GetByIdAndQuestion(cmd.Learning.Id.Value, null);
            return _learning != null;
        }

        private  bool QuestionIsUnique(StoreLearningRecordCommand cmd)
        {
            _learning =  _chatBotLearningProvider.GetByIdAndQuestion(null, cmd.Learning.Question);
            if (_learning == null || (cmd.Learning.Id.HasValue && _learning.Id == cmd.Learning.Id)) return true;
            cmd.ErrorMessage = "Такой вопрос уже есть в выборке ";
            if (_learning.CategoryId.HasValue)
            {
                var categ =  _chatBotCategoryProvider.GetByOriginId(_learning.CategoryId.Value);
                if (categ == null)
                    cmd.ErrorMessage += "(категория не найдена)";
                else
                    cmd.ErrorMessage += $"({categ.Name} - {categ.Partition.Title}/{categ.UpperPartition.Title})";
            }
            else
            {
                cmd.ErrorMessage += "(без категории)";
            }
            return false;
        }

    }
}
