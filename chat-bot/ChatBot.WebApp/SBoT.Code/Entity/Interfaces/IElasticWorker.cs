using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SBoT.Code.Dto;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IElasticWorker
    {
        void ReindexWords();
        List<ResponseDto> FindResponse(List<string> words, string context, int? mode);
    }
}
