using System.Runtime.Serialization;

namespace SBoT.Connect.Abstractions.Dto
{
    [DataContract]
    public class RosterConfigDto
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Keyword { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}
