using System.Collections.Generic;
using SBoT.Code.Dto;

namespace SBoT.Code.Services.Abstractions
{
    public interface IRosterService
    {
        Dictionary<string, RosterConfigDto> Roster();
        List<RosterDto> Find(string query, int skip, int take, string source);
        RosterDto GetByCode(string code, string source);
    }
}
