using System;
using System.Collections.Generic;
using System.Text;

namespace Sbtlife.Admin.ReadStorage.Common.Model.ChatBot
{
    public class WordDto : DtoIntBase
    {
        public int PatternId { get; set; }

        public string WordName { get; set; }

        public int? WordTypeId { get; set; }

        public List<WordFormDto> WordForms { get; set; }
    }
}
