using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Services.Abstractions;

namespace ChatBot.WebApp.Controllers
{
    [Route("roster")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [EnableCors("SiteCorsPolicy")]
    public class RosterController : Controller
    {
        private readonly IRosterService _rosterService;

        public RosterController(IRosterService rosterService)
        {
            _rosterService = rosterService;
        }

        [HttpPost("find")]
        public IActionResult Find([FromBody] FindRequestDto request)
        {
            var employees = _rosterService.Find(request.Query, request.Skip, request.Take, request.Source);

            return Json(employees);
        }
    }
}
