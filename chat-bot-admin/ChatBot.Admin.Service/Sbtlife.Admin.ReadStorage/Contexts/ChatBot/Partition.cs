using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class Partition : Entity
    {
        public Guid? ParentId { get; set; }

        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string ParentTitle { get; set; }

        public int SubpartitionCount { get; set; }
        public int CategoryCount { get; set; }
        public int CategoryPublishedCount { get; set; }
        public int LearningCount { get; set; }
    }
}
