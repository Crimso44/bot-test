using System;

namespace ChatBot.Admin.CommandHandlers.Model
{
    public class CommandHandlerInfo
    {
        public Type Interface { get; set; }
        public Type Command { get; set; }
    }
}
