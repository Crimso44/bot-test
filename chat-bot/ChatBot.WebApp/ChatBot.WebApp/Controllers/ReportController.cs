using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.WebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;

namespace ChatBot.WebApp.Controllers
{
    [Route("report")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IWordFormer _wordFormer;
        private readonly IChatter _chatter;
        private readonly IHostingEnvironment _env;
        private readonly IRoleChecker _role;
        private readonly IFileTransformer _fileTransformer;
        private readonly IServiceProvider _serviceProvider;

        public ReportController(IWordFormer wordFormer, IHostingEnvironment hostingEnvironment, IRoleChecker role, IChatter chatter,
            IFileTransformer fileTransformer, IServiceProvider serviceProvider)
        {
            _wordFormer = wordFormer;
            _env = hostingEnvironment;
            _role = role;
            _chatter = chatter;
            _fileTransformer = fileTransformer;
            _serviceProvider = serviceProvider;
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

        [HttpGet("mto")]
        public IActionResult GetReportMto(string from, string to)
        {
            _role.CheckIsReports();
            try
            {
                if (!DateTime.TryParseExact(from, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dFrom))
                    return HttpHelper.CreateResponseForError($"Неверный формат даты: {from}");

                if (!DateTime.TryParseExact(to, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dTo))
                    return HttpHelper.CreateResponseForError($"Неверный формат даты: {to}");

                var file = _chatter.GetReportMto(dFrom, dTo, _env.ContentRootPath);

                return HttpHelper.CreateResponseForFile(file);
            }
            catch (Exception e)
            {
                return HttpHelper.CreateResponseForError(e.Message);
            }
        }

        [HttpGet("mto/compare")]
        public IActionResult GetReportMtoCompare(string set)
        {
            _role.CheckIsReports();
            try
            {
                var file = _chatter.GetReportMtoCompare(set, _env.ContentRootPath);

                return HttpHelper.CreateResponseForFile(file);
            }
            catch (Exception e)
            {
                return HttpHelper.CreateResponseForError(e.Message);
            }
        }

        public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        [HttpGet("mto/answers")]
        public IActionResult GetReportMtoAnswers(string from, string to, int? threads)
        {
            _role.CheckIsReports();
            try
            {
                if (!DateTime.TryParseExact(from, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dFrom))
                    return HttpHelper.CreateResponseForError($"Неверный формат даты: {from}");

                if (!DateTime.TryParseExact(to, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dTo))
                    return HttpHelper.CreateResponseForError($"Неверный формат даты: {to}");

                FileDto file = null;
                var res01 = new List<ReportMtoDto>();

                if (threads.HasValue && threads.Value > 1)
                {
                    var data = _chatter.GetReportData(dFrom, dTo);
                    var datas = splitList(data, data.Count / threads.Value + 1).ToList();
                    while (datas.Count() < threads.Value) datas.Add(new List<ReportDto>());

                    var threadsList = new List<Thread>();
                    for (var i = 0; i < threads.Value; i++)
                    {
                        var chatter = (IChatter)_serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IChatterTransient));
                        var tc = new ThreadChatter(chatter, datas[i], (List<ReportMtoDto> result) =>
                        {
                            lock (res01)
                            {
                                res01.AddRange(result);
                            }
                        });
                        var t = new Thread(new ThreadStart(tc.ThreadProc));
                        t.Start();
                        threadsList.Add(t);
                    }
                    threadsList.ForEach(t => t.Join());

                    var bytes = _fileTransformer.MakeReportMtoAnswers(res01, _env.ContentRootPath);
                    file = new FileDto { Body = bytes, Name = "ReportMtoAnswers.xlsx" };
                }
                else
                {
                    file = _chatter.GetReportMtoAnswers(dFrom, dTo, _env.ContentRootPath);
                }

                return HttpHelper.CreateResponseForFile(file);
            }
            catch (Exception e)
            {
                return HttpHelper.CreateResponseForError(e.Message);
            }
        }


    }
}
