using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Admin.WebApi.Requests
{
    public class GetCategoryCollectionFilter : GetCollectionFilter
    {
        public string Pattern { get; set; }

        public string Answer { get; set; }

        public string Context { get; set; }

        public Guid? PartitionId { get; set; }
        public Guid? SubPartitionId { get; set; }

        public string ChangedBy { get; set; }

        public string SortColumn { get; set; }
        public bool SortDescent { get; set; }

        public bool IsDisabled { get; set; }
    }
}
