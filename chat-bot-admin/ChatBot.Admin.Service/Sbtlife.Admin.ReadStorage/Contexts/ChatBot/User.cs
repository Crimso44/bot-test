using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class User : Entity
    {
        public string SigmaLogin { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }
    }
}
