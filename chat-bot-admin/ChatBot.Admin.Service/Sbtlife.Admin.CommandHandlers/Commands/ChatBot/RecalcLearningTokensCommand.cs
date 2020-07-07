using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class RecalcLearningTokensCommand : CommandBase
    {
        public bool IsFullRecalc { get; set; }
    }
}
