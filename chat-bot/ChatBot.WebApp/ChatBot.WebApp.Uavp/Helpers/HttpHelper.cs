using System.IO;
using Microsoft.AspNetCore.Mvc;
using SBoT.Code.Uavp.Dto;

namespace ChatBot.WebApp.Uavp.Helpers
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
