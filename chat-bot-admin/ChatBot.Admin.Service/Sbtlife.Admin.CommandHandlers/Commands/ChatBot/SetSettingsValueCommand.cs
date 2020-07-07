using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Commands.ChatBot
{
    public class SetSettingsValueCommand : CommandBase
    {
        public List<SetSettingsNameValue> Settings { get; set; }
    }

    public class SetSettingsNameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

