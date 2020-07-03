using System.Collections.Generic;
using SBoT.Code.Dto;

namespace SBoT.Code.Repository.Interfaces
{
    public interface IUsefulLinksRepository
    {
        string FormatLinks(List<LinkDto> links, int limit);
        List<LinkDto> SearchLinks(string question);
    }
}
