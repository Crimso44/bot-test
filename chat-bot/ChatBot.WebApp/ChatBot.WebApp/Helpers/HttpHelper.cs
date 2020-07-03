using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SBoT.Code.Dto;

namespace ChatBot.WebApp.Helpers
{
    public static class HttpHelper
    {
        public static IActionResult CreateResponseForFile(FileDto file, string type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            var result = new FileStreamResult(new MemoryStream(file.Body), type) { 
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
