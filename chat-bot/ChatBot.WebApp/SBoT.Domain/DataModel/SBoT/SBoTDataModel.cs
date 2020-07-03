using SBoT.Domain.DataModel.SBoT.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using SBoT.Domain.Const;

namespace SBoT.Domain.DataModel.SBoT
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class SBoTDataModel : DbContext, ISBoTDataModel
    {
        public SBoTDataModel(DbContextOptions<SBoTDataModel> options) : base(options)
        {
        }

        public virtual DbSet<Partition> Partitions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Pattern> Patterns { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<PatternWordRel> PatternWordRels { get; set; }
        public virtual DbSet<WordForm> WordForms { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportStat> ReportStats { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BirthdayException> BirthdayExceptions { get; set; }
        public virtual DbSet<TestQuestions> TestQuestions { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Learning> Learnings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partition>().HasKey(x => x.Id);
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Pattern>().HasKey(x => x.Id);
            modelBuilder.Entity<Word>().HasKey(x => x.Id);
            modelBuilder.Entity<PatternWordRel>().HasKey(x => new { x.PatternId, x.WordId });
            modelBuilder.Entity<WordForm>().HasKey(x => x.Id);
            modelBuilder.Entity<History>().HasKey(x => x.Id);
            modelBuilder.Entity<Report>().HasKey(x => x.Id);
            modelBuilder.Entity<ReportStat>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<BirthdayException>().HasKey(x => x.EmployeeId);
            modelBuilder.Entity<TestQuestions>().HasKey(x => x.Id);
            modelBuilder.Entity<Config>().HasKey(x => x.Name);
            modelBuilder.Entity<Learning>().HasKey(x => x.Id);
        }

        public Task<List<Report>> GetReports(DateTime from, DateTime to)
        {
            var sql =
                "select \n" +
                "   h.Id, h.QuestionDate [Date], h.UserName FIO, '' Payload, '' TabNo, IsNull(NullIf(h.Context, ''), h.SetContext) Context, h.Question, h.OriginalQuestion,  \n" +
                "	case when NullIf(h.Answer, '') is null and not (h.AnswerText like '" + AppSettingsConst.SpecifyMessage + "%') then convert(bit,0) else convert(bit, 1) end IsAnswered, \n" +
                "    h.Answer, h.AnswerText, IsNull(h.[Like], 0) [Like], h.Context ContextIn, h.CategoryOriginId, h.IsMto, p.Name SubPartition, pp.Name Partition, h.MtoThresholds, h.SigmaLogin, \n" +
                "    case when h.QuestionDate >= convert(datetime,'29.04.2020', 104) and h.Source is null then 'Win' else h.Source end Source \n" +
                "from History h \n" +
                "left join Category c on c.OriginId = h.CategoryOriginId and c.IsTest = 0 \n" +
                "left join Partition p on c.PartitionId = p.Id \n" +
                "left join Partition pp on p.ParentId = pp.Id \n" +
                "where h.QuestionDate between @from and DateAdd(dd, 1, @to) \n" +
                "    and (IsNull(h.IsButton,0) = 0 or h.OriginalQuestion <> '(dislike)') \n" +
                "order by h.UserName, h.QuestionDate";

            return Reports.FromSql(sql, new SqlParameter("from", from), new SqlParameter("to", to)).ToListAsync();
        }

        public Task<List<ReportStat>> GetReportStats(DateTime from, DateTime to)
        {
            var sql =
                "select \n" +
                "    convert(int, ROW_NUMBER() OVER(ORDER BY count(*) desc, h.Question)) Id, h.Question, \n" +
                "    convert(float,sum(case when NullIf(h.Answer, '') is null and not (h.AnswerText like '" + AppSettingsConst.SpecifyMessage + "%')  then 0 else 1 end))/ count(*) IsAnswered, \n" +
                "	 case when sum(case when IsNull(h.[Like], 0) = 0 then 0 else 1 end) = 0 then null else convert(float,sum(IsNull(h.[Like], 0))) / sum(case when IsNull(h.[Like], 0) = 0 then 0 else 1 end) end[Like],  \n" +
                "    count(*) [Count] \n" +
                "from History h \n" +
                "where h.QuestionDate between @from and DateAdd(dd, 1, @to) \n" +
                "group by h.Question \n" +
                "order by count(*) desc, h.Question";
            return ReportStats.FromSql(sql, new SqlParameter("from", from), new SqlParameter("to", to)).ToListAsync();
        }

    }

    public class SBoTDataModelTransient : SBoTDataModel, ISBoTDataModelTransient
    {
        public SBoTDataModelTransient(DbContextOptions<SBoTDataModel> options) : base(options)
        {
        }

    }
}


