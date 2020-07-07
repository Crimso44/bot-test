using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot
{
    internal interface ISetSettingValueCommandValidator : IValidator<SetSettingValueCommand>
    {
    }
}
