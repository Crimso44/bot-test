using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class SetSettingValueCommand : CommandBase
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
