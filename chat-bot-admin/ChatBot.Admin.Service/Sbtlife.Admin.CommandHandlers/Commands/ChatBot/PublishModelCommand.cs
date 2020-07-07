using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class PublishModelCommand : CommandBase
    {
        public string ModelCommand { get; set; }
        public Guid ModelId { get; set; }
    }
}
