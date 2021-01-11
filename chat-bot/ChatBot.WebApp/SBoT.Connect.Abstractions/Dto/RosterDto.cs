using System;
using System.Runtime.Serialization;

namespace SBoT.Connect.Abstractions.Dto
{
    [DataContract]
    public class RosterDto
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }
    }

}
