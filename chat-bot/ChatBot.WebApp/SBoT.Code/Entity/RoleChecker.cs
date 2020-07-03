using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Services.Abstractions;
using Um.Connect.Abstractions;
using Um.Abstractions.ChatBot;
using CoreRoleConst = Um.Abstractions.Core.Const.RoleConst;

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
            return _user.User().Roles.Any(x => x.Id == RoleConst.ChatBotAdministrator || x.Id == CoreRoleConst.Administrator);
        }

        public bool IsReports()
        {
            return _user.User().Roles.Any(x => x.Id == RoleConst.ChatBotReports || x.Id == RoleConst.ChatBotAdministrator || x.Id == CoreRoleConst.Administrator);
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
