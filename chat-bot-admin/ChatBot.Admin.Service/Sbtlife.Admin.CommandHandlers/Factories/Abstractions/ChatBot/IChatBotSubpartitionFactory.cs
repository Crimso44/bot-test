using System;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot
{
    interface IChatBotSubpartitionFactory
    {
        PartitionDto GetSubpartition();
        PartitionOptionalDto GetEditableSubpartition(Guid id);
    }
}
