using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eve
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
        public int? Mode;

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
