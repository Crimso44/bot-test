using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatBot.Admin.Common.Model.ChatBot
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
