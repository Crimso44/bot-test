using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class ChatBotWordValidator : CommandValidatorBase<WordDto>
    {
        private readonly IWordService _wordService;

        public ChatBotWordValidator(IWordService wordService)
        {
            _wordService = wordService;

            RuleFor(cmd => cmd).Must(CheckWordForms).WithMessage(cmd => $"Слово {cmd.WordName}: неправильная часть речи");
        }


        private bool CheckWordForms(WordDto word)
        {
            var errors = new List<string>();
            _wordService.FillWordForms(word, errors);
            return !errors.Any() && word.WordForms.All(x => !string.IsNullOrEmpty(x.Form));
        }
    }
}

