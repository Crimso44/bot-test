using System;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Specifications.ChatBot
{
    public class GetLearningSpecification : GetCollectionSpecification, ISpecification
    {
        public SortingDto[] Sorting { get; set; }
        public FilteringDto[] Filtering { get; set; }

        public Pair<Guid>[] Category { get; set; }
        public Guid? PartitionId { get; set; }
        public Guid? UpperPartitionId { get; set; }
    }
}
