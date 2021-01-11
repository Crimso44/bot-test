using AutoMapper;
using SBoT.Code.Uavp.DataModel.Cross;
using SBoT.Code.Uavp.DataModel.Cross.Interfaces;
using SBoT.Code.Uavp.Dto;
using SBoT.Code.Uavp.Services.Abstractions;
using SBoT.Connect.Abstractions.Dto;
using SBoT.Connect.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBoT.Code.Uavp.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ICrossDataModel _crossDataModel;

        private Dictionary<string, StaffDto> _staffs = new Dictionary<string, StaffDto>();

        public UserInfoService(ICrossDataModel crossDataModel)
        {
            _crossDataModel = crossDataModel;
        }

        public async Task<IUser> GetUserInfo(string sigmaLogin)
        {
            return new UserDto()
            {
                Id = Guid.Empty,
                Name = sigmaLogin,
                SigmaLogin = sigmaLogin
            };
        }
        
        public async Task<string> DecodeValue(string sigmaLogin, string category, Dictionary<string, RosterDto> roster, bool isDefault)
        {
            var res = isDefault ? "Не найдено" : "";
            var user = roster.ContainsKey("E") ? roster["E"] : new RosterDto() { Id = Guid.Empty, Code = sigmaLogin, Name = sigmaLogin, Source = "E" };
            switch (category.ToLower())
            {
                case Classes.Const.OrgKeys.FIO:
                    res = GetStaff(user.Code).EmplName;
                    break;
                case Classes.Const.OrgKeys.Position:
                    res = GetStaff(user.Code).EmplPos;
                    break;
                case Classes.Const.OrgKeys.City:
                    res = GetStaff(user.Code).City;
                    break;
            }
            return res;
        }

        private StaffDto GetStaff(string code)
        {
            if (!_staffs.ContainsKey(code))
            {
                var staff = _crossDataModel.Staff
                    .FirstOrDefault(x => x.Active && !string.IsNullOrEmpty(x.EmplNo) && x.EmplNo == code);
                if (staff == null)
                {
                    _staffs[code] = new StaffDto();
                }
                else
                {
                    _staffs[code] = Mapper.Map<StaffDto>(staff);
                }
            }
            return _staffs[code];
        }
    }
}
