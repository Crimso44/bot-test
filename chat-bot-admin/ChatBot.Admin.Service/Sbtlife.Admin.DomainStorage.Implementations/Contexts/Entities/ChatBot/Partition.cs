using System;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class Partition
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }

    }
}
