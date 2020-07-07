using System;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Specifications.ChatBot
{
    public class GetModelSpecification : GetCollectionSpecification, ISpecification
    {
        public SortingDto[] Sorting { get; set; }
        public FilteringDto[] Filtering { get; set; }
    }
}
