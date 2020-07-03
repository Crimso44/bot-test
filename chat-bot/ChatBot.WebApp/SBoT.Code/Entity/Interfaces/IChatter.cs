using SBoT.Code.Classes;
using SBoT.Code.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IChatter
    {
        Guid Id();
        AnswerDto AskBot(string source, string question, string context);
        AnswerDto AskBotEx(string source, string question, string context, bool isSilent, bool isMto, bool isCheckSbt);
        AnswerDto AskBotByButton(string source, string question, string context, string category, bool isCheckSbt);
        AnswerDto AskBotByButtonMail(string source, string mail, string question, string category);
        AnswerDto AskBotByMail(string source, string question, string mail);
        bool SetLike(Pair<int> like);
        string GetUserFirstName();

        FileDto GetReportMto(DateTime from, DateTime to, string rootPath);
        FileDto GetReportMtoCompare(string set, string rootPath);
        FileDto GetReportMtoAnswers(DateTime from, DateTime to, string rootPath);
        List<ReportDto> GetReportData(DateTime from, DateTime to);
    }

    public interface IChatterTransient : IChatter {};
}
