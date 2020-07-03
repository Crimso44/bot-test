using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Classes
{
    public class Config
    {
        public bool IsElastic { get; set; }
        public bool IsRabbitMQ { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsUseMto { get; set; }
        public bool IsCheckSbt { get; set; }
    }
}
