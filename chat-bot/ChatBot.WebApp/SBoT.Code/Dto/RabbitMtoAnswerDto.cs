using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class RabbitMtoAnswerDto
    {
        [DataMember]
        public string question { get; set; }
        [DataMember]
        public ProbabilityDto[] answer { get; set; }
        [DataMember]
        public string answerName { get; set; }
        [DataMember]
        public float? distance { get; set; }
        [DataMember]
        public string nearestAnswerId { get; set; }
        public string message { get; set; }
    }

    [DataContract]
    public class ProbabilityDto
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public float rate { get; set; }
    }
}
