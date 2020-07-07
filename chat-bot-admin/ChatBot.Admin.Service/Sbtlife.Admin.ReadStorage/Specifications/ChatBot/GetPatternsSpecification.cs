using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;
using System;

namespace ChatBot.Admin.ReadStorage.Specifications.ChatBot
{
    public class GetPatternsSpecification : GetCollectionSpecification, ISpecification
    {
        public SortingDto[] Sorting { get; set; }
        public FilteringDto[] Filtering { get; set; }

        public Pair<int>[] Category { get; set; }
        public Guid? PartitionId { get; set; }
        public Guid? UpperPartitionId { get; set; }
    }
}
