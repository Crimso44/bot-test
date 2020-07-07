using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Eve
{
    public class WebRequestDto
    {
        public string Url { get; set; }
        public object Parameters { get; set; }
    }

    [DataContract]
    public class RequestDto
    {
        [DataMember]
        public RequestDataDto variables;
    }

    [DataContract]
    public class RequestDataDto
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Category { get; set; }
    }

    [DataContract]
    public class FindRequestDto
    {
        [DataMember]
        public string Query { get; set; }
        [DataMember]
        public int Skip { get; set; }
        [DataMember]
        public int Take { get; set; }
        [DataMember]
        public string Source { get; set; }
    }

    [DataContract]
    public class RosterDto
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }
    }

}
