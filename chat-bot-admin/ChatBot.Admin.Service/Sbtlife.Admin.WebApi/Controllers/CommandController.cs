using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatBot.Admin.CommandHandlers.Model;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.CommandHandlers.Services.Abstractions;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.WebApi.Const;
using ChatBot.Admin.WebApi.ViewModel;
using System.Reflection;

namespace ChatBot.Admin.WebApi.Controllers
{
    [Route("commands")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CommandController : CommandControllerBase
    {
        private readonly ILogger<CommandController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandTypeProviderService _commandTypeProviderService;
        private readonly IJsonSerializerService _jsonSerializerService;

        public CommandController(
            ILogger<CommandController> logger,
            IServiceProvider serviceProvider,
            ICommandTypeProviderService commandTypeProviderService,
            IJsonSerializerService jsonSerializerService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _commandTypeProviderService = commandTypeProviderService;
            _jsonSerializerService = jsonSerializerService;
        }

        [HttpPost("check")]
        public  IActionResult HandleCheck([FromBody] CommandRequestDto commandRequest)
        {
            var commandHandlerInfo =
                _commandTypeProviderService.GetCommandInfoById($"ICheck{commandRequest.Name}CommandHandler");

            if (commandHandlerInfo == null)
            {
                _logger.LogError(
                    $"Интерфейс \"ICheck{commandRequest.Name}CommandHandler\" для команды \"{commandRequest.Name}\" не найден.");
                return BadRequest(CreateBadRequestBody(MessageConst.CommandHandleError));
            }

            return  HandleCommand(() => Handle(commandRequest, commandHandlerInfo), _logger);
        }

        [HttpPost]
        public  IActionResult HandleCmd([FromBody] CommandRequestDto commandRequest)
        {
            var commandHandlerInfo =
                _commandTypeProviderService.GetCommandInfoById($"I{commandRequest.Name}CommandHandler");

            if (commandHandlerInfo == null)
            {
                _logger.LogError(
                    $"Интерфейс \"I{commandRequest.Name}CommandHandler\" для команды \"{commandRequest.Name}\" не найден.");
                return BadRequest(CreateBadRequestBody(MessageConst.CommandHandleError));
            }

            return  HandleCommand(() => Handle(commandRequest, commandHandlerInfo), _logger);
        }

        private  ICommandResult Handle(CommandRequestDto commandRequest, CommandHandlerInfo commandHandlerInfo)
        {
            var commandHandlerInstance = _serviceProvider.GetService(commandHandlerInfo.Interface);

            if (commandHandlerInstance == null)
                throw new InvalidOperationException($"Реализация интерфейса команды \"{commandHandlerInfo.Interface}\" не зарегистрирована в контейнере.");

            var command = _jsonSerializerService.JObjectToObject(commandHandlerInfo.Command, commandRequest.Payload) as ICommand;

            if (command == null)
                throw new InvalidOperationException($"Объект команды \"{commandHandlerInfo.Command}\" не поддерживает интерфейс ICommand.");

            command.SetId(commandRequest.Id);

            var commandHandlerMethod = commandHandlerInstance.GetType().GetMethod("Handle");

            if (commandHandlerMethod == null)
                throw new InvalidOperationException($"Не найден метод \"Handle\" в объекте \"{commandHandlerInstance}\".");

            try
            {
                return (ICommandResult)commandHandlerMethod.Invoke(commandHandlerInstance, new object[] { command });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

    }
}
