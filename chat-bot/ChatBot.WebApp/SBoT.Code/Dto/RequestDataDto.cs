using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class RequestDataDto
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Source { get; set; }
    }
}
