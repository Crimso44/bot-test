using System;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Model.Command;
using Um.Connect.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Factories.Commands
{
    class CommandFactory : ICommandFactory
    {
        private readonly IUser _user;
        private readonly IDateTimeService _dateTimeService;

        public CommandFactory(IUser user,
            IDateTimeService dateTimeService)
        {
            _user = user;
            _dateTimeService = dateTimeService;
        }

        public CommandDto GetCommand(Guid id, Guid typeId, int version)
        {
            return new CommandDto
            {
                Id = id,
                TypeId = typeId,
                Version = version,
                RegisteredBy = _user.Id,
                RegisteredOnUtc = _dateTimeService.SessionUtcNow
            };
        }
    }
}
