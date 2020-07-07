using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class Learning : EntityInt
    {
        public string Question { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PartitionId { get; set; }
        public string Tokens { get; set; }
    }
}
