using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatBot.Admin.CommandHandlers.CommandValidators;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions;
using ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.CommandValidators.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Abstractions.Commands;
using ChatBot.Admin.CommandHandlers.Factories.ChatBot;
using ChatBot.Admin.CommandHandlers.Factories.Commands;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions.ChatBot;
using ChatBot.Admin.CommandHandlers.Handlers.Implementations.ChatBot;
using ChatBot.Admin.CommandHandlers.Services.Abstractions;
using ChatBot.Admin.CommandHandlers.Services.Implementations;

namespace ChatBot.Admin.CommandHandlers
{
    public static class CommandModuleBootstraper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            // CommandHandlers
            serviceCollection.AddScoped<ICreateChatBotCategoryCommandHandler, CreateChatBotCategoryCommandHandler>();
            serviceCollection.AddScoped<IEditChatBotCategoryCommandHandler, EditChatBotCategoryCommandHandler>();
            serviceCollection.AddScoped<IDeleteChatBotCategoryCommandHandler, DeleteChatBotCategoryCommandHandler>();
            serviceCollection.AddScoped<ICreateChatBotPartitionCommandHandler, CreateChatBotPartitionCommandHandler>();
            serviceCollection.AddScoped<IEditChatBotPartitionCommandHandler, EditChatBotPartitionCommandHandler>();
            serviceCollection.AddScoped<IDeleteChatBotPartitionCommandHandler, DeleteChatBotPartitionCommandHandler>();
            serviceCollection.AddScoped<ICreateChatBotSubpartitionCommandHandler, CreateChatBotSubpartitionCommandHandler>();
            serviceCollection.AddScoped<IEditChatBotSubpartitionCommandHandler, EditChatBotSubpartitionCommandHandler>();
            serviceCollection.AddScoped<IDeleteChatBotSubpartitionCommandHandler, DeleteChatBotSubpartitionCommandHandler>();
            serviceCollection.AddScoped<IChatBotPublishCategoriesCommandHandler, ChatBotPublishCategoriesCommandHandler>();
            serviceCollection.AddScoped<IChatBotUnpublishCategoriesCommandHandler, ChatBotUnpublishCategoriesCommandHandler>();

            serviceCollection.AddScoped<IStorePatternCommandHandler, StorePatternCommandHandler>();
            serviceCollection.AddScoped<IDeletePatternCommandHandler, DeletePatternCommandHandler>();

            serviceCollection.AddScoped<IStoreLearningRecordCommandHandler, StoreLearningRecordCommandHandler>();
            serviceCollection.AddScoped<IStoreLearnModelReportCommandHandler, StoreLearnModelReportCommandHandler>();
            serviceCollection.AddScoped<IDeleteLearningRecordCommandHandler, DeleteLearningRecordCommandHandler>();
            serviceCollection.AddScoped<IRecalcLearningTokensCommandHandler, RecalcLearningTokensCommandHandler>();
            serviceCollection.AddScoped<ILearnModelCommandHandler, LearnModelCommandHandler>();
            serviceCollection.AddScoped<IPublishModelCommandHandler, PublishModelCommandHandler>();
            serviceCollection.AddScoped<ICopyPatternToLearnCommandHandler, CopyPatternToLearnCommandHandler>();

            serviceCollection.AddScoped<ISetSettingValueCommandHandler, SetSettingValueCommandHandler>();
            serviceCollection.AddScoped<ISetSettingsValueCommandHandler, SetSettingsValueCommandHandler>();

            // Services
            serviceCollection.AddSingleton<ICommandTypeProviderService, CommandTypeProviderService>();

            // CommandValidators
            serviceCollection.AddScoped<ICreateChatBotCategoryCommandValidator, CreateChatBotCategoryCommandValidator>();
            serviceCollection.AddScoped<IEditChatBotCategoryCommandValidator, EditChatBotCategoryCommandValidator>();
            serviceCollection.AddScoped<IDeleteChatBotCategoryCommandValidator, DeleteChatBotCategoryCommandValidator>();
            serviceCollection.AddScoped<ICreateChatBotPartitionCommandValidator, CreateChatBotPartitionCommandValidator>();
            serviceCollection.AddScoped<IEditChatBotPartitionCommandValidator, EditChatBotPartitionCommandValidator>();
            serviceCollection.AddScoped<IDeleteChatBotPartitionCommandValidator, DeleteChatBotPartitionCommandValidator>();
            serviceCollection.AddScoped<ICreateChatBotSubpartitionCommandValidator, CreateChatBotSubpartitionCommandValidator>();
            serviceCollection.AddScoped<IEditChatBotSubpartitionCommandValidator, EditChatBotSubpartitionCommandValidator>();
            serviceCollection.AddScoped<IDeleteChatBotSubpartitionCommandValidator, DeleteChatBotSubpartitionCommandValidator>();
            serviceCollection.AddScoped<IChatBotPublishCategoriesCommandValidator, ChatBotPublishCategoriesCommandValidator>();
            serviceCollection.AddScoped<IChatBotUnpublishCategoriesCommandValidator, ChatBotUnpublishCategoriesCommandValidator>();

            serviceCollection.AddScoped<IStorePatternCommandValidator, StorePatternCommandValidator>();
            serviceCollection.AddScoped<IDeletePatternCommandValidator, DeletePatternCommandValidator>();

            serviceCollection.AddScoped<IStoreLearningRecordCommandValidator, StoreLearningRecordCommandValidator>();
            serviceCollection.AddScoped<IStoreLearnModelReportCommandValidator, StoreLearnModelReportCommandValidator>();
            serviceCollection.AddScoped<IDeleteLearningRecordCommandValidator, DeleteLearningRecordCommandValidator>();
            serviceCollection.AddScoped<IRecalcLearningTokensCommandValidator, RecalcLearningTokensCommandValidator>();
            serviceCollection.AddScoped<ILearnModelCommandValidator, LearnModelCommandValidator>();
            serviceCollection.AddScoped<IPublishModelCommandValidator, PublishModelCommandValidator>();
            serviceCollection.AddScoped<ICopyPatternToLearnCommandValidator, CopyPatternToLearnCommandValidator>();

            serviceCollection.AddScoped<ISetSettingValueCommandValidator, SetSettingValueCommandValidator>();
            serviceCollection.AddScoped<ISetSettingsValueCommandValidator, SetSettingsValueCommandValidator>();

            // Factories
            serviceCollection.AddScoped<ICommandFactory, CommandFactory>();
            serviceCollection.AddScoped<IChatBotCategoryFactory, ChatBotCategoryFactory>();
            serviceCollection.AddScoped<IChatBotPartitionFactory, ChatBotPartitionFactory>();
            serviceCollection.AddScoped<IChatBotSubpartitionFactory, ChatBotSubpartitionFactory>();
        }
    }
}
