using System;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;
using SBoT.Connect.Abstractions.Interfaces;

namespace ChatBot.Admin.CommandHandlers.Factories.ChatBot
{
    class ChatBotCategoryFactory : IChatBotCategoryFactory
    {
        private readonly IUser _user;
        private readonly IDateTimeService _dateTimeService;

        public ChatBotCategoryFactory(IUser user,
            IDateTimeService dateTimeService)
        {
            _user = user;
            _dateTimeService = dateTimeService;
        }

        public CategoryDto GetCategory()
        {
            return new CategoryDto
            {
                ChangedBy = _user.SigmaLogin,
                ChangedOn = _dateTimeService.SessionUtcNow.ToLocalTime(),
                OriginId = Guid.NewGuid(),
                IsTest = true,
                IsDisabled = false,
                IsAdded = true,
                IsIneligible = false,
                RequiredRoster = null
            };
        }

        public CategoryOptionalDto GetEditableCategory(int id)
        {
            return new CategoryOptionalDto
            {
                Id = id,
                IsTest = true,
                IsDisabled = false,
                IsChanged = true,
                ChangedBy = _user.SigmaLogin,
                ChangedOn = _dateTimeService.SessionUtcNow.ToLocalTime()
            };
        }
    }
}
