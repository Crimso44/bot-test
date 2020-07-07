﻿using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetCategoryCollection : IQuery<GetCategoryCollectionSpecification, CollectionDto<CategoryDto>>
    {
    }
}

