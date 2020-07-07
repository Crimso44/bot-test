using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Admin.ReadStorage.Model;

namespace ChatBot.Admin.WebApi.Helpers
{
    public static class HttpHelper
    {
        public static IActionResult CreateResponseForFile(FileDto file, string type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            var result = new FileStreamResult(new MemoryStream(file.Body), type)
            {
                FileDownloadName = file.Name
            };

            return result;
        }

        public static IActionResult CreateResponseForError(string message)
        {
            return new ContentResult()
            {
                Content = message
            };
        }


    }
}
