using System.Collections.Generic;

namespace SBoT.Code.Dto
{
    public class ResponseListDto
    {
        public List<ResponseDto> Responses;
        public List<LinkDto> Links;
        public string NewQuestion;
        public string ModelResponse;
        public bool IsTransliterated;
        public bool IsDizzy;
        public bool IsMtoAnswer;
        public string MtoThresholds;
    }
}
