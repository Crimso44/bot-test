using System.Collections.Generic;
using SBoT.Code.Dto;

namespace SBoT.Code.Services.Abstractions
{
    public interface IMessageHistoryService
    {
        List<HistoryDto> GetHistoryFrameForCurrentUser(int beforeId, int size);
    }
}
