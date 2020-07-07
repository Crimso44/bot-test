using System;
using System.Collections.Generic;
using System.Text;

namespace Sbtlife.Admin.ReadStorage.Common.Model.ChatBot
{
    public class PartitionDto : DtoBase
    {
        public Guid? ParentId { get; set; }

        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string ParentTitle { get; set; }

        public List<PartitionDto> Subpartitions { get; set; }

        public int SubpartitionCount { get; set; }
        public int CategoryCount { get; set; }
        public int CategoryPublishedCount { get; set; }
    }
}
