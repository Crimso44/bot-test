using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IFileTransformService
    {
        byte[] MakeXls(CategoryDto[] rows, string rootPath);
    }
}
