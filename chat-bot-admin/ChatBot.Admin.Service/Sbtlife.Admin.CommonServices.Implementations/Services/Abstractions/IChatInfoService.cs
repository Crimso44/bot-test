using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IChatInfoService
    {
        List<string> OrgData();
        Dictionary<string, RosterConfigDto> Roster();
        string RosterName(string roster);
        UserDto GetUserInfo(string sigmaLogin);
        List<UserDto> GetUsersInfo(List<string> sigmaLogins);
    }
}
