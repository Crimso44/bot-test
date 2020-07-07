using System;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Specifications
{
    public class GetItemIntSpecification : ISpecification
    {
        public GetItemIntSpecification(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
