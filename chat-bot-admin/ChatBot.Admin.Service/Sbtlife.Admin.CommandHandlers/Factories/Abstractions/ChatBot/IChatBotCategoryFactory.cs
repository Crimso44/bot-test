using System;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;

namespace ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot
{
    interface IChatBotCategoryFactory
    {
        CategoryDto GetCategory();
        CategoryOptionalDto GetEditableCategory(int id);
    }
}
