using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBoT.Code.Dto
{
    public class ResponseDto
    {
        public string Category;
        public string Response;
        public string SetContext;
        public int? SetMode;
        public string Context;
        public int? Mode;
        public decimal Rate;
        public decimal ContextRate;

        public int CategoryId;
        public Guid? CategoryOriginId;
        public string CategoryRequiredRoster;
        public int PatternId;
        public int PatternCnt;
        public string PatternPhrase;
        public int FoundCnt;

        public bool IsDefault;
        public bool IsMto;
        public string MtoThresholds;

        public List<WeightDto> Weights;
    }
}
