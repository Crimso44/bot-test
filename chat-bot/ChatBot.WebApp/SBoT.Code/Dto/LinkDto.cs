using System.Runtime.Serialization;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class LinkDto
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}
