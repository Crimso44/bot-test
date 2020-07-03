using System;
using System.Collections.Generic;
using System.Text;
using Um.Connect.Abstractions;

namespace SBoT.Code.Services.Abstractions
{
    public interface IUserInfoService
    {
        IUser User();
        void SetCurrentUserByMail(string mail);
    }
}
