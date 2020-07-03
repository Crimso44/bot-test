using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Services.Abstractions;
using Um.Connect.Abstractions;

namespace SBoT.Code.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IOptions<Urls> _urls;
        private readonly IWebRequestProcess _request;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private UserDto _currentUser = null;

        public UserInfoService(IOptions<Urls> urls, IWebRequestProcess request, IHttpContextAccessor httpContextAccessor)
        {
            _urls = urls;
            _request = request;
            _httpContextAccessor = httpContextAccessor;
        }

        public IUser User()
        {
            if (_currentUser == null)
            {
                var sigmaLogin = _httpContextAccessor.HttpContext?.User?.Identity.Name;
                if (string.IsNullOrEmpty(sigmaLogin)) return null;

                if (string.IsNullOrEmpty(_urls.Value.ChatInfo))
                {
                    _currentUser = new UserDto() { Name = sigmaLogin, SigmaLogin = sigmaLogin };
                }
                else
                {
                    var us = _request.WebApiRequestGet<UserDtoSerializable>($"{_urls.Value.ChatInfo}/info/user", new Dictionary<string, object> {{"sigmaLogin", sigmaLogin}});
                    _currentUser = new UserDto() { Id = us.Id, Name = us.Name, SigmaEmail = us.SigmaEmail, SigmaLogin = us.SigmaLogin, Roles = us.Roles.ToList<IRole>() };
                }
            }
            return _currentUser;
        }

        public void SetCurrentUserByMail(string mail)
        {
            var us = _request.WebApiRequestGet<UserDtoSerializable>($"{_urls.Value.ChatInfo}/info/user", new Dictionary<string, object> { { "sigmaLogin", mail } });
            _currentUser = new UserDto() { Id = us.Id, Name = us.Name, SigmaEmail = us.SigmaEmail, SigmaLogin = us.SigmaLogin, Roles = us.Roles.ToList<IRole>() };
        }
    }
}
