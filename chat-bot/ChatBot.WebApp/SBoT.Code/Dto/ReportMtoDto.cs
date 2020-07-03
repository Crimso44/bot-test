using System;
using System.Collections.Generic;
using System.Text;
using SBoT.Code.Classes;

namespace SBoT.Code.Dto
{
    public class ReportMtoDto
    {
        public string Question { get; set; }
        public bool IsMtoAnswer { get; set; }
        public bool IsChanged { get; set; }
        public List<Pair<Guid>> OriginalCategorysElastic { get; set; }
        public List<Pair<Guid>> OriginalCategorys { get; set; }
        public string ModelResponse { get; set; }
    }
}
