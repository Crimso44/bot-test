using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBoT.Code.Dto
{
    public class PatternDto
    {
        public int? Id { get; set; }

        public int CategoryId { get; set; }

        public string Context { get; set; }
        public bool? OnlyContext { get; set; }

        public string Phrase { get; set; }

        public int? WordCount { get; set; }

        public List<WordDto> Words { get; set; }
    }
}
