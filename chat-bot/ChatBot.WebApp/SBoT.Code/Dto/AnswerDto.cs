using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SBoT.Code.Classes;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class AnswerDto
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Title;

        [DataMember]
        public string Context;

        [DataMember]
        public decimal Rate;

        [DataMember]
        public bool IsLikeable;

        [DataMember]
        public bool IsMto;

        public bool IsMtoAnswer;
        public string QuestionChanged;
        public string ModelResponse;
        public List<Pair<Guid>> OriginalCategorys;
    }
}
