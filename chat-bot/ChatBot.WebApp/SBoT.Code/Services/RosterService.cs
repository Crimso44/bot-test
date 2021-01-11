using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository.Interfaces;
using SBoT.Code.Services.Abstractions;
using SBoT.Connect.Abstractions.Dto;

namespace SBoT.Code.Services
{
    internal class RosterService : IRosterService
    {
        private readonly IWebRequestProcess _request;
        private readonly IOptions<Urls> _urls;

        private Dictionary<string, RosterConfigDto> _roster;

        public RosterService(IWebRequestProcess request, IOptions<Urls> urls)
        {
            _request = request;
            _urls = urls;
        }



        public Dictionary<string, RosterConfigDto> Roster()
        {
            if (_roster == null) 
            {
                if (string.IsNullOrEmpty(_urls.Value.ChatInfo)) return new Dictionary<string, RosterConfigDto>();
                _roster = _request.WebApiRequestGet<Dictionary<string, RosterConfigDto>>($"{_urls.Value.ChatInfo}/info/roster", new Dictionary<string, object>());
            }
            return _roster;
        }


        public List<RosterDto> Find(string query, int skip, int take, string source)
        {
            if (string.IsNullOrEmpty(_urls.Value.ChatInfo)) return null;
            var res = _request.WebApiRequestGet<List<RosterDto>>($"{_urls.Value.ChatInfo}/info/roster/find", new Dictionary<string, object>
            {
                { "q", query },
                { "skip", skip },
                { "take", take },
                { "source", source }
            });

            return res;
        }

        public RosterDto GetByCode(string code, string source)
        {
            if (string.IsNullOrEmpty(_urls.Value.ChatInfo)) return null;
            var res = _request.WebApiRequestGet<RosterDto>($"{_urls.Value.ChatInfo}/info/roster/get", new Dictionary<string, object>
            {
                { "code", code },
                { "source", source }
            });

            return res;
        }

    }
}
