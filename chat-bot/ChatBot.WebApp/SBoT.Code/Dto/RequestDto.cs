using SBoT.Code.Classes;
using System.Runtime.Serialization;

namespace SBoT.Code.Dto
{
    [DataContract]
    public class RequestDto
    {
        [DataMember]
        public RequestDataDto variables;
    }
}
