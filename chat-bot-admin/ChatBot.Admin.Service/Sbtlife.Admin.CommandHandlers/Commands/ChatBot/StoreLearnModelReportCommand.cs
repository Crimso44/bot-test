using System;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Rabbit.Model;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class StoreLearnModelReportCommand : CommandBase
    {
        public Guid ModelId { get; set; }
        public LearningModelAnswerDto Report { get; set; }
        public string FullAnswer { get; set; }
    }
}
