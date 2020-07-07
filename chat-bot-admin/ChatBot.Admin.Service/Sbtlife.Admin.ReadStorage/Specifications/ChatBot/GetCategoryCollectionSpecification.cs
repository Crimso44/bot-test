using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.ReadStorage.Specifications.ChatBot
{
    public class GetCategoryCollectionSpecification : GetCollectionSpecification
    {
        public string Pattern { get; set; }

        public string Answer { get; set; }

        public string Context { get; set; }

        public Guid? PartitionId { get; set; }
        public Guid? SubPartitionId { get; set; }

        public string ChangedBy { get; set; }

        public bool IsDisabled { get; set; }
    }
}
