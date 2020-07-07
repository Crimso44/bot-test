
using System;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Specifications
{
    public class GetCollectionSpecification : ISpecification
    {
        public string Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? IsArchived { get; set; }
        public string SortColumn { get; set; }
        public bool SortDescent { get; set; }
    }
}
