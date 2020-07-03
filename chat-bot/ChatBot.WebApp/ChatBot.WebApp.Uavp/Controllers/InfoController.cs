﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SBoT.Code.Uavp.Dto;
using SBoT.Code.Uavp.Services.Abstractions;
using uavpConst = SBoT.Code.Uavp.Classes.Const;
using Um.Connect.Abstractions;

namespace ChatBot.WebApp.Uavp.Controllers
{
    [Route("info")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class InfoController : Controller
    {
        private readonly IUserInfoService _infoService;
        private readonly IRosterService _rosterService;
        private readonly ISearchService _searchService;

        public InfoController(IUserInfoService infoService, IRosterService rosterService, ISearchService searchService)
        {
            _infoService = infoService;
            _rosterService = rosterService;
            _searchService = searchService;
        }

        [HttpGet("user")]
        public async Task<IUser> GetUserData(string sigmaLogin)
        {
            return await _infoService.GetUserInfo(sigmaLogin);
        }

        [HttpPost("decode")]
        public async Task<string> DecodeValue([FromBody] Dictionary<string, object> data)
        {
            var sigmaLogin = (string)data["sigmaLogin"];
            var category = (string)data["category"];
            var roster = JObject.FromObject(data["roster"]).ToObject<Dictionary<string, RosterDto>>();
            var isDefault = (bool)data["isDefault"];
            return await _infoService.DecodeValue(sigmaLogin, category, roster, isDefault);
        }

        [HttpGet("decode/list")]
        public List<string> DecodeList()
        {
            return new List<string>(uavpConst.OrgData.Keys);
        }

        [HttpGet("find")]
        public List<LinkDto> FindData(string question)
        {
            return null;
            // отключаем, больше искать в полезных ссылках не надо
            //return _searchService.Search(question);
        }


        [HttpGet("roster")]
        public Dictionary<string, RosterConfigDto> GetRoster()
        {
            return _rosterService.GetRoster();
        }


        [HttpGet("roster/find")]
        public List<RosterDto> Find(string q, int skip, int take, string source)
        {
            var employees = _rosterService.Find(q, skip, take, source);

            return employees;
        }

        [HttpGet("roster/get")]
        public RosterDto GetByCode(string code, string source)
        {
            var employee = _rosterService.GetByCode(code, source);

            return employee;
        }
    }
}
