using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;

namespace ChatBot.Admin.ReadStorage.Contexts.Abstractions
{
    interface IChatBotReadonlyContext
    {
        DbSet<BirthdayException> BirthdayExceptions { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<History> Histories { get; set; }
        DbSet<Learning> Learnings { get; set; }
        DbSet<ModelLearning> ModelLearnings { get; set; }
        DbSet<ModelLearningReport> ModelLearningReports { get; set; }
        DbSet<ModelLearningConf> ModelLearningConfs { get; set; }
        DbSet<ModelLearningMarkup> ModelLearningMarkups { get; set; }
        DbSet<Partition> Partitions { get; set; }
        DbSet<Pattern> Patterns { get; set; }
        DbSet<PatternWordRel> PatternWordRels { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Word> Words { get; set; }
        DbSet<WordForm> WordForms { get; set; }
        DbSet<Config> Configs { get; set; }
    }
}
