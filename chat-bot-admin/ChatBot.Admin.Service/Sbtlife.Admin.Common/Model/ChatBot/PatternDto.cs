using System.Collections.Generic;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    public class PatternDto
    {
        public int? Id { get; set; }

        public int CategoryId { get; set; }

        public string Context { get; set; }
        public bool? OnlyContext { get; set; }
        public int? Mode { get; set; }

        public string Phrase { get; set; }

        public int? WordCount { get; set; }

        public List<WordDto> Words { get; set; }
    }
}
