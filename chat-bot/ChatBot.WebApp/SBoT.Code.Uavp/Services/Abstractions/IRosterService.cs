using SBoT.Connect.Abstractions.Dto;
using System.Collections.Generic;

namespace SBoT.Code.Uavp.Services.Abstractions
{
    public interface IRosterService
    {
        Dictionary<string, RosterConfigDto> GetRoster();

        List<RosterDto> Find(string q, int skip, int take, string source);

        RosterDto GetByCode(string code, string source);
    }
}
