using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.Common.Extensions;
using ChatBot.Admin.Common.Helpers;
using ChatBot.Admin.Common.Model;
using ChatBot.Admin.WebApi.Const;
using ChatBot.Admin.WebApi.ViewModel;

namespace ChatBot.Admin.WebApi.Controllers
{
    public abstract class CommandControllerBase : Controller
    {
        protected IActionResult CommandProcessed(ICommandResult result)
        {
            var response = new CommandResponseDto
            {
                Error = result.Error,
                Text = result.Text,
                Payload = result.Payload
            };

            if (response.Error)
                return BadRequest(response);

            return Ok(response);
        }

        protected CommandResponseDto CreateBadRequestBody(string text, CommonError[] errors = null)
        {
            if (errors == null)
                errors = CommonErrorHelper.CreateError(text).ToValueArray();

            return new CommandResponseDto { Error = true, Text = text, Payload = errors };
        }

        protected  IActionResult HandleCommand(Func<ICommandResult> func, ILogger logger)
        {
            try
            {
                var result =  func();
                return CommandProcessed(result);
            }

            catch (BusinessLogicException e)
            {
                return BadRequest(CreateBadRequestBody(e.Message, e.Errors));
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(CreateBadRequestBody(e.Message/*MessageConst.CommandHandleError*/));
            }
        }
    }
}
