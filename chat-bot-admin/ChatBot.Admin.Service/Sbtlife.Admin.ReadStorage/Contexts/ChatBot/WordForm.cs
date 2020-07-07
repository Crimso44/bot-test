using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class WordForm : EntityInt
    {
        public int WordId { get; set; }

        public string Form { get; set; }
    }
}
