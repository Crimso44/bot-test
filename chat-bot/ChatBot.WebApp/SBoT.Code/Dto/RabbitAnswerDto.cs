using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class RabbitAnswerDto<T>
    where T: class 
    {
        [DataMember]
        public Guid task_id { get; set; }
        [DataMember]
        public T result { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string traceback { get; set; }
        [DataMember]
        public string[] children { get; set; }
    }
}
