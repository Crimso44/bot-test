using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class Word : EntityInt
    {
        public string WordName { get; set; }

        public int? WordTypeId { get; set; }
    }
}
