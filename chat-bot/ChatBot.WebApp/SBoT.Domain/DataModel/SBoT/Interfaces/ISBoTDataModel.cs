using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBoT.Domain.DataModel.SBoT.Interfaces
{
    public interface ISBoTDataModel
    {
        DbSet<Partition> Partitions { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Pattern> Patterns { get; set; }
        DbSet<Word> Words { get; set; }
        DbSet<PatternWordRel> PatternWordRels { get; set; }
        DbSet<WordForm> WordForms { get; set; }
        DbSet<History> History { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<ReportStat> ReportStats { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<BirthdayException> BirthdayExceptions { get; set; }
        DbSet<TestQuestions> TestQuestions { get; set; }
        DbSet<Config> Configs { get; set; }
        DbSet<Learning> Learnings { get; set; }

        Task<List<Report>> GetReports(DateTime from, DateTime to);
        Task<List<ReportStat>> GetReportStats(DateTime from, DateTime to);

        int SaveChanges();
        DatabaseFacade Database { get; }
    }

    public interface ISBoTDataModelTransient : ISBoTDataModel { };

}
