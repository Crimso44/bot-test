using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot
{
    internal class PublishModelCommandValidator : CommandValidatorBase<PublishModelCommand>, IPublishModelCommandValidator
    {
    }
}
