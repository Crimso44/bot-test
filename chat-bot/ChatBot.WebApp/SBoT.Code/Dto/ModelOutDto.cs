using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class ModelOutWordsDto
    {
        [DataMember]
        public string word { get; set; }
        [DataMember]
        public int count { get; set; }
    }

}
