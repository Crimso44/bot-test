using System;
using System.Globalization;
using ChatBot.WebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SBoT.Code.Entity.Interfaces;

namespace ChatBot.WebApp.Controllers
{
    [Route("edit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class EditingController : Controller
    {
        private readonly IWordFormer _wordFormer;
        private readonly IRabbitWorker _rabbitWorker;
        private readonly IElasticWorker _elasticWorker;
        private readonly IHostingEnvironment _env;
        private readonly IRoleChecker _role;

        public EditingController(IWordFormer wordFormer, IHostingEnvironment hostingEnvironment, IRoleChecker role, IRabbitWorker rabbitWorker, 
            IElasticWorker elasticWorker)
        {
            _wordFormer = wordFormer;
            _env = hostingEnvironment;
            _role = role;
            _rabbitWorker = rabbitWorker;
            _elasticWorker = elasticWorker;
        }

        [HttpGet("report")]
        public IActionResult GetReport(string from, string to)
        {
            _role.CheckIsReports();
            try
            {
                if (!DateTime.TryParseExact(from, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dFrom))
                    return HttpHelper.CreateResponseForError($"Неверный формат даты: {from}");

                if (!DateTime.TryParseExact(to, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dTo))
                    return HttpHelper.CreateResponseForError($"Неверный формат даты: {to}");

                var file = _wordFormer.GetReport(dFrom, dTo, _env.ContentRootPath);

                return HttpHelper.CreateResponseForFile(file);
            }
            catch (Exception e)
            {
                return HttpHelper.CreateResponseForError(e.Message);
            }
        }


    }
}
