using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetCategoryStat : IQuery<GetCategoryCollectionSpecification, string>
    {
    }
}
