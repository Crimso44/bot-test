using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetChatBotLearningCollection : IQuery<GetLearningSpecification, CollectionDto<LearningDto>>
    {
    }
}
