using System.Collections.Generic;

namespace SBoT.Code.Dto
{
    public class WordDto
    {
        public int? Id { get; set; }

        public int PatternId { get; set; }

        public string WordName { get; set; }

        public int? WordTypeId { get; set; }

        public List<WordFormDto> WordForms { get; set; }
    }
}
