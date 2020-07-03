using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("Config")]
    public class Config
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
