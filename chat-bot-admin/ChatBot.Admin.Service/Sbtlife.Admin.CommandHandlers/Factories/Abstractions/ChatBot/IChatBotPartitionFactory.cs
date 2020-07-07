using System;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot
{
    interface IChatBotPartitionFactory
    {
        PartitionDto GetPartition();
        PartitionOptionalDto GetEditablePartition(Guid id);
    }
}
