using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot
{
    internal interface ILearnModelCommandValidator : IValidator<LearnModelCommand>
    {
    }

}
