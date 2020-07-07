using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class ChatBotUnpublishCategoriesCommand : CommandBase
    {
        public Guid? PartitionId { get; set; }
        public Guid? SubPartId { get; set; }
    }
}
