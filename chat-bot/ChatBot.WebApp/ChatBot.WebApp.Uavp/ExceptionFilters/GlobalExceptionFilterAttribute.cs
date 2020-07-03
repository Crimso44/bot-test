using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ChatBot.WebApp.Uavp.ExceptionFilters
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());

            context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            throw context.Exception;
        }
    }
}
