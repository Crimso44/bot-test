using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class DeleteLearningRecordCommand : CommandBase
    {
        public int LearningId { get; set; }
    }
}
