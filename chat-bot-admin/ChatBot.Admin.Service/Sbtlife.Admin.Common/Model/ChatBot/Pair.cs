using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    [DataContract]
    public class Pair<T>
    {
        [DataMember]
        public T Id;

        [DataMember]
        public string Title;
    }
}
