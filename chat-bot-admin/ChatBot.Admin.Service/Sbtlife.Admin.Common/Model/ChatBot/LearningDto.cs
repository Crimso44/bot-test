using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    public class LearningDto
    {
        public int? Id { get; set; }
        public string Question { get; set; }
        public string Tokens { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PartitionId { get; set; }
        public string CategoryName { get; set; }
        public Guid? CategoryPartitionId { get; set; }
        public Guid? CategoryUpperPartitionId { get; set; }
    }
}
