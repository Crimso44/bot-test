using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot
{
    public interface IChatBotSettingsProvider
    {
        void UpdateSettingValue(string name, string value);
    }
}
