using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatBot.Admin.Common.Rabbit.Model
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
