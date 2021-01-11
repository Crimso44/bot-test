using SBoT.Connect.Abstractions.Interfaces;

namespace SBoT.Code.Services.Abstractions
{
    public interface IUserInfoService
    {
        IUser User();
        void SetCurrentUserByMail(string mail);
    }
}
