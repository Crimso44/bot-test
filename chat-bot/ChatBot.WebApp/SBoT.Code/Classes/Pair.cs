using System.Runtime.Serialization;

namespace SBoT.Code.Classes
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
