using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.WebApi.Controllers.ChatBot
{
    [Route("chatbotwords")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChatBotWordController : Controller
    {
        private readonly IWordService _wordService;

        public ChatBotWordController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpPost("word/forms")]
        public IActionResult GetWordForms([FromBody] WordDto word)
        {
            _wordService.FillWordForms(word, null);
            return Json(word);
        }

        [HttpPost("pattern/calculate")]
        public IActionResult PatternCalculate([FromBody] PatternDto pattern)
        {
            var res = _wordService.PatternCalculate(pattern);
            return Json(res);
        }

    }
}
