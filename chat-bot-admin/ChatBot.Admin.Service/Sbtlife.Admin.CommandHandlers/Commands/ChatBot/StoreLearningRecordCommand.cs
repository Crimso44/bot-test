using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class StoreLearningRecordCommand : CommandBase
    {
        public LearningDto Learning { get; set; }
    }
}
