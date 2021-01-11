using SBoT.Connect.Abstractions.Dto;
using System.Collections.Generic;

namespace SBoT.Code.Services.Abstractions
{
    public interface IInfoService
    {
        string DecodeValue(string category, Dictionary<string, RosterDto> roster, bool isDefault, string sigmaLogin);
    }
}
