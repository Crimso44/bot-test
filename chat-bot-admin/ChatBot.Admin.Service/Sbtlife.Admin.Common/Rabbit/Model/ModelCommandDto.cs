using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatBot.Admin.Common.Rabbit.Model
{
    [DataContract]
    public class ModelCommandDto
    {
        [DataMember]
        public string Command { get; set; }
        [DataMember]
        public string Data { get; set; }
    }
}
