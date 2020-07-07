using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class ChatBotPatternValidator : CommandValidatorBase<PatternDto>
    {
        private readonly IWordService _wordService;

        public ChatBotPatternValidator(IWordService wordService)
        {
            _wordService = wordService;

            RuleFor(cmd => cmd.Phrase).NotEmpty().WithMessage("Не задан паттерн");
            RuleFor(cmd => cmd.Words).Must(x => x != null && x.Count > 0).WithMessage("Нет слов в паттерне");
            RuleFor(cmd => cmd.Words).SetCollectionValidator(new ChatBotWordValidator(_wordService));
        }

    }
}
