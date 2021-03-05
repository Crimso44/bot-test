using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class Pattern : EntityInt
    {
        public int CategoryId { get; set; }

        public string Phrase { get; set; }

        public int? WordCount { get; set; }

        public string Context { get; set; }
        public bool? OnlyContext { get; set; }
        public int? Mode { get; set; }

        public Guid? CategoryOriginId { get; set; }
        public string CategoryName { get; set; }
        public Guid? CategoryPartitionId { get; set; }
        public Guid? CategoryUpperPartitionId { get; set; }
        public bool? IsTest { get; set; }
    }
}
