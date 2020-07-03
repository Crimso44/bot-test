using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace ChatBot.WebApp.Controllers
{
    [Route("sbot")]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [EnableCors("SiteCorsPolicy")]
    public class SbotController : Controller
    {
        private readonly IChatter _chatter;
        private readonly IRoleChecker _role;
        private readonly IMessageHistoryService _messageHistoryService;

        public SbotController(IChatter chatter, IRoleChecker role, IMessageHistoryService messageHistoryService)
        {
            _chatter = chatter;
            _role = role;
            _messageHistoryService = messageHistoryService;
        }

        [HttpPost("ask")]
        public AnswerDto AskBot([FromBody] RequestDto question)
        {
            return _chatter.AskBot(question.variables.Source, question.variables.Title, question.variables.Id);
        }

        [HttpGet("ask")]
        public AnswerDto AskBot(string question, string context)
        {
            return _chatter.AskBot("Get", question, context);
        }

        [HttpGet("askByMail")]
        public AnswerDto AskBotByMail(string source, string mail, string question)
        {
            return _chatter.AskBotByMail(source, question, mail);
        }

        [HttpGet("askByButtonMail")]
        public AnswerDto AskByButton(string source, string mail, string question, string category)
        {
            return _chatter.AskBotByButtonMail(source, mail, question, category);
        }

        [HttpPost("askByButton")]
        public AnswerDto AskByButton([FromBody] RequestDto question)
        {
            return _chatter.AskBotByButton(question.variables.Source, question.variables.Title, question.variables.Id, question.variables.Category, false);
        }

        [HttpPost("setLike")]
        public bool SetLike([FromBody] Pair<int> like)
        {
            return _chatter.SetLike(like);
        }

        [HttpGet("user")]
        public string GetUserName()
        {
            return _role.GetUserName();
        }

        [HttpGet("name")]
        public string GetUserFirstName()
        {
            return _chatter.GetUserFirstName();
        }

        [HttpGet("history/{beforeId}")]
        public List<HistoryDto> GetHistoryFrameForCurrentUser(int beforeId, [FromQuery] int? count)
        {
            return _messageHistoryService.GetHistoryFrameForCurrentUser(beforeId, count ?? 10);
        }


    }
}
