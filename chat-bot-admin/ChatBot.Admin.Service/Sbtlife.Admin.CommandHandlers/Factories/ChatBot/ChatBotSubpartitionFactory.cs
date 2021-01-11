using System;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;
using SBoT.Connect.Abstractions.Interfaces;

namespace ChatBot.Admin.CommandHandlers.Factories.ChatBot
{
    class ChatBotSubpartitionFactory : IChatBotSubpartitionFactory
    {
        private readonly IUser _user;
        private readonly IDateTimeService _dateTimeService;

        public ChatBotSubpartitionFactory(IUser user,
            IDateTimeService dateTimeService)
        {
            _user = user;
            _dateTimeService = dateTimeService;
        }

        public PartitionDto GetSubpartition()
        {
            return new PartitionDto
            {
                Id = Guid.NewGuid()
            };
        }

        public PartitionOptionalDto GetEditableSubpartition(Guid id)
        {
            return new PartitionOptionalDto
            {
                Id = id
            };
        }
    }
}
