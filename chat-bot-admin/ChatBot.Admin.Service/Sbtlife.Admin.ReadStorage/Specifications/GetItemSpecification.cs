using System;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Specifications
{
    public class GetItemSpecification : ISpecification
    {
        public GetItemSpecification(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
