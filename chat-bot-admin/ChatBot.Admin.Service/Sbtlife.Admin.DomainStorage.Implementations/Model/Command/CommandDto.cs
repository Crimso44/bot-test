using System;
using ChatBot.Admin.DomainStorage.Model.Abstractions;

namespace ChatBot.Admin.DomainStorage.Model.Command
{
    public class CommandDto : DtoBase
    {
        public Guid TypeId { get; set; }
        public int Version { get; set; }
        public string Payload { get; set; }
        public Guid RegisteredBy { get; set; }
        public DateTime RegisteredOnUtc { get; set; }
    }
}
