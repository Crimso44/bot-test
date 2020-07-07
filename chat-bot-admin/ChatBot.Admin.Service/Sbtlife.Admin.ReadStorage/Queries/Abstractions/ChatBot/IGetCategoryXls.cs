using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetCategoryXls : IQuery<GetItemStringSpecification, FileDto>
    {
    }
}
