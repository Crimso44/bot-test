using SBoT.Code.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Services.Abstractions
{
    public interface IInfoService
    {
        string DecodeValue(string category, Dictionary<string, RosterDto> roster, bool isDefault, string sigmaLogin);
    }
}
