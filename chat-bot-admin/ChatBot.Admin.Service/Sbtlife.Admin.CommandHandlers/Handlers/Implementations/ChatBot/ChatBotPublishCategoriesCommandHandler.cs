using System;
using System.Threading.Tasks;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Const;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Model.Abstractions;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.Common.Rabbit.Abstractions;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;

namespace ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot
{
    internal class ChatBotPublishCategoriesCommandHandler : CommandHandlerBase<ChatBotPublishCategoriesCommand>, IChatBotPublishCategoriesCommandHandler
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IChatBotCategoryFactory _categoryFactory;
        private readonly IChatBotCategoryProvider _categoryProvider;
        private readonly IDictInPublisher _dictInPublisher;
        private readonly IElasticService _elasticService;

        public ChatBotPublishCategoriesCommandHandler(ICommandFactory commandFactory,
            ICommandProvider commandProvider,
            IChatBotPublishCategoriesCommandValidator validator,
            IJsonSerializerService jsonSerializerService,
            IPermissionsService permissionsService,
            IChatBotCategoryFactory categoryFactory,
            IChatBotCategoryProvider categoryProvider,
            IDictInPublisher dictInPublisher,
            IElasticService elasticService)
            : base(commandFactory, commandProvider, validator, jsonSerializerService)
        {
            _permissionsService = permissionsService;
            _categoryFactory = categoryFactory;
            _categoryProvider = categoryProvider;
            _dictInPublisher = dictInPublisher;
            _elasticService = elasticService;
        }

        public  ICommandResult Handle(ChatBotPublishCategoriesCommand command)
        {
            if (!_permissionsService.CanEditChatBot)
                throw new UnauthorizedAccessException();

             CheckAndStoreCommand(command, CommandTypeConst.ChatBotCategory.Create, 1);

             _categoryProvider.PublishCategories(command.PartitionId, command.SubPartId);

            if (_dictInPublisher.Channel != null)
            {
                var data =  _categoryProvider.GetCategoriesWords();
                var dto = data.ToArray();
                _dictInPublisher.SendMessage(Guid.NewGuid(), dto);
            }

            var words =  _categoryProvider.GetWordsForIndex();
            _elasticService.ReindexWords(words);

            return Ok(MessageConst.CategoryPublished);
        }
    }
}
