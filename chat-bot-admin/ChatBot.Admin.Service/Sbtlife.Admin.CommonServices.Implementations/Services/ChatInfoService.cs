using System;
using System.Collections.Generic;
using System.Linq;
using ChatBot.Admin.Common.Classes;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace ChatBot.Admin.CommonServices.Services
{
    public class ChatInfoService : IChatInfoService
    {

        private readonly IOptions<Urls> _urls;
        private readonly IWebRequestProcess _request;

        private List<string> _orgData = null;
        private Dictionary<string, RosterConfigDto> _roster;

        public ChatInfoService(IOptions<Urls> urls, IWebRequestProcess request)
        {
            _urls = urls;
            _request = request;
        }

        public List<string> OrgData()
        {
            if (_orgData == null)
            {
                _orgData = _request.WebApiRequestGet<List<string>>($"{_urls.Value.ChatInfo}/info/decode/list");
            }

            return _orgData;
        }

        public UserDto GetUserInfo(string sigmaLogin)
        {
            return new UserDto() { SigmaLogin = sigmaLogin }; //!!!

            var us = _request.WebApiRequestGet<UserDtoSerializable>($"{_urls.Value.ChatInfo}/info/user", new Dictionary<string, object> { { "sigmaLogin", sigmaLogin } });
            if (us == null) return new UserDto() { SigmaLogin = sigmaLogin };

            var user = new UserDto() { Id = us.Id, Name = us.Name, SigmaEmail = us.SigmaEmail, SigmaLogin = us.SigmaLogin };

            return user;
        }

        public List<UserDto> GetUsersInfo(List<string> sigmaLogins)
        {
            var res = sigmaLogins.Select(GetUserInfo).ToList();
            return res;
        }

        public Dictionary<string, RosterConfigDto> Roster()
        {
            if (_roster == null)
            {
                _roster = _request.WebApiRequestGet<Dictionary<string, RosterConfigDto>>($"{_urls.Value.ChatInfo}/info/roster", new Dictionary<string, object>());
            }

            return _roster;
        }

        public string RosterName(string roster)
        {
            if (string.IsNullOrEmpty(roster)) return null;

            return Roster().ContainsKey(roster.Trim()) ? Roster()[roster.Trim()].Name : null;

        }

    }
}
