using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal class SetSettingsValueCommandValidator : CommandValidatorBase<SetSettingsValueCommand>, ISetSettingsValueCommandValidator
    {
        public SetSettingsValueCommandValidator()
        {
            /*RuleFor(cmd => cmd.Name).NotEmpty().WithMessage(RequiredFieldMessage("Название"));
            RuleFor(cmd => cmd.Value).NotEmpty().WithMessage(RequiredFieldMessage("Значение"));*/
        }



    }
}
