using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IWordService
    {
        void FillWordForms(WordDto word, List<string> errors);
        PatternDto PatternCalculate(PatternDto pattern);
    }
}
