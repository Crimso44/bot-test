using System;
using System.Collections.Generic;
using SBoT.Code.Dto;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IFileTransformer
    {
        byte[] MakeReport(List<ReportDto> rows, List<ReportStatDto> rows2, string rootPath);
        byte[] MakeReportMto(List<ReportSpellDto> rows01, int cntAll, TimeSpan time0, TimeSpan time1, string rootPath);
        byte[] MakeReportMtoCompare(List<ReportMtoDto> rows01, int cntChanged, TimeSpan time0, TimeSpan time1, string rootPath);
        byte[] MakeReportMtoAnswers(List<ReportMtoDto> rows01, string rootPath);
    }
}
