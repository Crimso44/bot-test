using System;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
