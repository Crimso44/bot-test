using System;
using System.Collections.Generic;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    public class PartitionDto 
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string ParentTitle { get; set; }

        public List<PartitionDto> Subpartitions { get; set; }

        public int SubpartitionCount { get; set; }
        public int CategoryCount { get; set; }
        public int CategoryPublishedCount { get; set; }
        public int LearningCount { get; set; }
    }
}
