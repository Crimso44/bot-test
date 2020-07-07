using System;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Specifications
{
    public class GetItemStringSpecification : ISpecification
    {
        public GetItemStringSpecification(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
