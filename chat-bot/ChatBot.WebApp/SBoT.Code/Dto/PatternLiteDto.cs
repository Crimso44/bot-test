using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class PatternLiteDto
    {
        [DataMember]
        public string Phrase { get; set; }

        [DataMember]
        public List<string> Words { get; set; }
    }
}
