using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class Learning : EntityInt
    {
        public string Question { get; set; }
        public string Tokens { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PartitionId { get; set; }
        public string CategoryName { get; set; }
        public Guid? CategoryPartitionId { get; set; }
        public Guid? CategoryUpperPartitionId { get; set; }
    }
}
