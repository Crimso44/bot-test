using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatBot.Admin.Common.Const;
using ChatBot.Admin.ReadStorage.Contexts;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot;
using ChatBot.Admin.ReadStorage.Queries.ChatBot;

namespace ChatBot.Admin.ReadStorage
{
    public static class ReadModuleBootstraper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            // Context
            serviceCollection.AddDbContext<ChatBotReadonlyContext>(o => o.UseSqlServer(configuration[AppSettingsConst.ConnectionStrings.ChatBot]));
            serviceCollection.AddScoped<IChatBotReadonlyContext>(provider => provider.GetRequiredService<ChatBotReadonlyContext>());

            // Queries
            serviceCollection.AddScoped<IGetCategoryCollection, GetCategoryCollection>();
            serviceCollection.AddScoped<IGetCategoryItem, GetCategoryItem>();
            serviceCollection.AddScoped<IGetPatternItem, GetPatternItem>();
            serviceCollection.AddScoped<IGetCategoryStat, GetCategoryStat>();
            serviceCollection.AddScoped<IGetCategoryXls, GetCategoryXls>();
            serviceCollection.AddScoped<IGetChatBotPartitionCollection, GetChatBotPartitionCollection>();
            serviceCollection.AddScoped<IGetChatBotPartitionDictionaryCollection, GetChatBotPartitionDictionaryCollection>();
            serviceCollection.AddScoped<IGetChatBotPartitionItem, GetChatBotPartitionItem>();
            serviceCollection.AddScoped<IGetChatBotSubpartitionCollection, GetChatBotSubpartitionCollection>();
            serviceCollection.AddScoped<IGetChatBotSubpartitionDictionaryCollection, GetChatBotSubpartitionDictionaryCollection>();
            serviceCollection.AddScoped<IGetChatBotSubpartitionItem, GetChatBotSubpartitionItem>();
            serviceCollection.AddScoped<IGetChatBotChangersDictionaryCollection, GetChatBotChangersDictionaryCollection>();
            serviceCollection.AddScoped<IGetChatBotSettingsCollection, GetChatBotSettingsCollection>();
            serviceCollection.AddScoped<IGetChatBotHistoryCollection, GetChatBotHistoryCollection>();
            serviceCollection.AddScoped<IGetChatBotLearningCollection, GetChatBotLearningCollection>();
            serviceCollection.AddScoped<IGetChatBotPatternsCollection, GetChatBotPatternsCollection>();
            serviceCollection.AddScoped<IGetChatBotModelCollection, GetChatBotModelCollection>();
        }
    }
}
