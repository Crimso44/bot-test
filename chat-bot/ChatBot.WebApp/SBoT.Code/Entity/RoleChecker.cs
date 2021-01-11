using System;
using System.Linq;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Services.Abstractions;
using RoleConst = SBoT.Connect.Abstractions.RoleConst;

namespace SBoT.Code.Entity
{
    public class RoleChecker : IRoleChecker
    {
        private readonly IUserInfoService _user;

        public RoleChecker(IUserInfoService user)
        {
            _user = user;
        }


        public bool IsAdmin()
        {
            return _user.User().Roles.Any(x => x.Id == RoleConst.ChatBotAdministrator);
        }

        public bool IsReports()
        {
            return _user.User().Roles.Any(x => x.Id == RoleConst.ChatBotReports || x.Id == RoleConst.ChatBotAdministrator);
        }

        public void CheckIsAdmin()
        {
            if (!IsAdmin())
                throw new Exception("Требуются права администратора");
        }

        public void CheckIsReports()
        {
            if (!IsReports())
                throw new Exception("Требуются права для получения отчетов");
        }

        public string GetUserName()
        {
            return _user.User().Name;
        }

    }
}
