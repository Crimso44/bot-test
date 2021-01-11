using System;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;
using SBoT.Connect.Abstractions.Interfaces;

namespace ChatBot.Admin.CommandHandlers.Factories.ChatBot
{
    class ChatBotPartitionFactory : IChatBotPartitionFactory
    {
        private readonly IUser _user;
        private readonly IDateTimeService _dateTimeService;

        public ChatBotPartitionFactory(IUser user,
            IDateTimeService dateTimeService)
        {
            _user = user;
            _dateTimeService = dateTimeService;
        }

        public PartitionDto GetPartition()
        {
            return new PartitionDto
            {
                Id = Guid.NewGuid()
            };
        }

        public PartitionOptionalDto GetEditablePartition(Guid id)
        {
            return new PartitionOptionalDto
            {
                Id = id
            };
        }
    }
}
