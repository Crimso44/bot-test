using System;
using ChatBot.Admin.DomainStorage.Model.Command;

namespace ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands
{
    interface ICommandFactory
    {
        CommandDto GetCommand(Guid id, Guid typeId, int version);
    }
}
