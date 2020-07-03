using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class WordListDto
    {
        [DataMember]
        public List<string> Words { get; set; }

        public bool IsDizzy;
    }
}
