using System;

namespace ChatBot.Admin.ReadStorage.Contexts.Abstractions
{
    abstract class Entity
    {
        public Guid Id { get; set; }
    }
}
