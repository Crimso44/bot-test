using Microsoft.AspNetCore.Mvc;

namespace ChatBot.WebApp.Uavp.Helpers
{
    public static class HttpHelper
    {
        public static IActionResult CreateResponseForError(string message)
        {
            return new ContentResult()
            {
                Content = message
            };
        }


    }
}
