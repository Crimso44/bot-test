using ChatBot.Admin.Common.Model.ChatBot;
using System;

namespace ChatBot.Admin.WebApi.Requests
{
    public class GetPatternsFilter
    {
        public string Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public SortingDto[] Sorting { get; set; }
        public FilteringDto[] Filtering { get; set; }

        public Pair<int>[] Category { get; set; }
        public Guid? PartitionId { get; set; }
        public Guid? UpperPartitionId { get; set; }
    }
}
