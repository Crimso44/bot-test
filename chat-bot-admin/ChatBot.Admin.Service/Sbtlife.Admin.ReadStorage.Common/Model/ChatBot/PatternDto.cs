using System;
using System.Collections.Generic;
using System.Text;

namespace Sbtlife.Admin.ReadStorage.Common.Model.ChatBot
{
    public class PatternDto : DtoIntBase
    {
        public int CategoryId { get; set; }

        public string Context { get; set; }
        public bool? OnlyContext { get; set; }

        public string Phrase { get; set; }

        public int? WordCount { get; set; }

        public List<WordDto> Words { get; set; }
    }
}
