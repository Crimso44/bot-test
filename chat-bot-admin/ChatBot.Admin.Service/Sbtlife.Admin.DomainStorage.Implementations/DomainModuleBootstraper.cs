using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Contexts;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.DocumentStorage;
using ChatBot.Admin.DomainStorage.Providers.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Commands;
using ChatBot.Admin.DomainStorage.Providers.DocumentStorage;

namespace ChatBot.Admin.DomainStorage
{
    public static class DomainModuleBootstraper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            // Context
            serviceCollection.AddDbContext<ChatBotContext>(o => o.UseSqlServer(configuration[AppSettingsConst.ConnectionStrings.ChatBot]));
            serviceCollection.AddDbContext<DocumentStorageContext>(o => o.UseSqlServer(configuration[AppSettingsConst.ConnectionStrings.DocumentStorage]));

            // Providers
            serviceCollection.AddScoped<ICommandProvider, CommandProvider>();

            serviceCollection.AddScoped<IChatBotCategoryProvider, ChatBotCategoryProvider>();
            serviceCollection.AddScoped<IChatBotPartitionProvider, ChatBotPartitionProvider>();
            serviceCollection.AddScoped<IChatBotSettingsProvider, ChatBotSettingsProvider>();
            serviceCollection.AddScoped<IChatBotLearningProvider, ChatBotLearningProvider>();
            serviceCollection.AddScoped<IDocumentStorageProvider, DocumentStorageProvider>();

        }
    }
}
