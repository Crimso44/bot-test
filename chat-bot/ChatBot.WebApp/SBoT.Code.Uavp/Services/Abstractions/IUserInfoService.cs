using SBoT.Connect.Abstractions.Dto;
using SBoT.Connect.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBoT.Code.Uavp.Services.Abstractions
{
    public interface IUserInfoService
    {
        Task<IUser> GetUserInfo(string sigmaLogin);
        Task<string> DecodeValue(string sigmaLogin, string category, Dictionary<string, RosterDto> roster, bool isDefault);
    }
}
