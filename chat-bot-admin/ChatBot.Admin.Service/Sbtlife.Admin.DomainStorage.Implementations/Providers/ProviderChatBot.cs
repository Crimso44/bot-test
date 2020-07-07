using ChatBot.Admin.DomainStorage.Contexts;

namespace ChatBot.Admin.DomainStorage.Providers
{
    internal abstract class ProviderChatBot
    {
        protected ProviderChatBot(ChatBotContext storage)
        {
            Context = storage;
        }

        protected ChatBotContext Context { get; }
    }
}
