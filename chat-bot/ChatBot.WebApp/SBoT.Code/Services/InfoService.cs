using Microsoft.Extensions.Options;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Services.Abstractions;
using SBoT.Connect.Abstractions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Services
{
    public class InfoService : IInfoService
    {
        private readonly IOptions<Urls> _urls;
        private readonly IWebRequestProcess _request;
        private readonly IUserInfoService _user;

        public InfoService(IOptions<Urls> urls, IWebRequestProcess request, IUserInfoService user)
        {
            _urls = urls;
            _request = request;
            _user = user;
        }

        public string DecodeValue(string category, Dictionary<string, RosterDto> roster, bool isDefault, string sigmaLogin)
        {
            if (string.IsNullOrEmpty(_urls.Value.ChatInfo)) return null;
            var res = _request.WebApiRequestPost<string>($"{_urls.Value.ChatInfo}/info/decode", new Dictionary<string, object> {
                { "sigmaLogin", string.IsNullOrEmpty(sigmaLogin) ? _user.User().SigmaLogin : sigmaLogin },
                { "category", category },
                { "roster", roster ?? new Dictionary<string, RosterDto>() },
                { "isDefault", isDefault }
            });

            return res;
        }

    }
}
