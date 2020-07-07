using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.DomainStorage.Contexts;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot;
using System.Linq;

namespace ChatBot.Admin.DomainStorage.Providers.ChatBot
{
    internal class ChatBotSettingsProvider : ProviderChatBot, IChatBotSettingsProvider
    {

        public ChatBotSettingsProvider(ChatBotContext storage)
            : base(storage)
        {
        }

        public  void UpdateSettingValue(string name, string value)
        {
            var entity =  Context.Configs.FirstOrDefault(x => x.Name == name);
            if (entity == null)
            {
                entity = new Config() {Name = name, Value = value};
                 Context.Configs.Add(entity);
            }
            else
            {
                entity.Value = value;
            }
             Context.SaveChanges();
        }

    }
}
