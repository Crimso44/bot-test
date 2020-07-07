using System;
using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;
using Microsoft.Extensions.Logging;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class StoreLearnModelReportCommandHandler : CommandHandlerBase<StoreLearnModelReportCommand>, IStoreLearnModelReportCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotLearningProvider _chatBotLearningsProvider;
        private readonly ILogger<StoreLearnModelReportCommandHandler> _logger;

        public StoreLearnModelReportCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IStoreLearnModelReportCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotLearningProvider chatBotLearningsProvider,
            ILogger<StoreLearnModelReportCommandHandler> logger)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _chatBotLearningsProvider = chatBotLearningsProvider;
            _logger = logger;
        }

        public  ICommandResult Handle(StoreLearnModelReportCommand command)
        {
            //if (!_permissionsService.CanEditChatBot)
            //    throw new UnauthorizedAccessException();
            try
            {
                 CheckAndStoreCommand(command, CommandTypeConst.ChatBotLearning.StoreReport, 1)
                    ;
                 _chatBotLearningsProvider.StoreLearningReport(command.ModelId, command.Report, command.FullAnswer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "StoreLearnModelReportCommandHandler error");
                throw e;
            }

            return Ok(MessageConst.ChatBotLearningStored);
        }
    }
}
