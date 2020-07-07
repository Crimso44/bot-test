using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    public class PatternsDto : PatternDto
    {
        public Guid? PartitionId { get; set; }
        public string CategoryName { get; set; }
        public Guid? CategoryPartitionId { get; set; }
        public Guid? CategoryUpperPartitionId { get; set; }
    }
}
