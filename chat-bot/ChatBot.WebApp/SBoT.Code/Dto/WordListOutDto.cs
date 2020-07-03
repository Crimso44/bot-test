using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class WordListOutDto
    {
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public Guid? CategoryOriginId { get; set; }
        [DataMember]
        public List<PatternLiteDto> Patterns { get; set; }
    }
}
