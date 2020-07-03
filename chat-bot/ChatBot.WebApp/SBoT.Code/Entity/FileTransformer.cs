using System;
using System.Collections.Generic;
using OfficeOpenXml;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using System.IO;
using System.Linq;
using LingvoNET;
using System.Text.RegularExpressions;
using System.Web;

namespace SBoT.Code.Entity
{
    public class FileTransformer : IFileTransformer
    {
        private MemoryStream OpenFile(string templateFileName, string rootPath, out string startPath)
        {
            startPath = rootPath;
            if (!startPath.EndsWith("\\")) startPath += "\\";
            startPath += "Resources\\Templates\\";

            byte[] byteArray = System.IO.File.ReadAllBytes(startPath + templateFileName);
            MemoryStream mem = new MemoryStream();
            mem.Write(byteArray, 0, (int)byteArray.Length);
            return mem;
        }

        public byte[] MakeReport(List<ReportDto> rows, List<ReportStatDto> rows2, string rootPath)
        {
            using (var package = new ExcelPackage())
            {
                var templateFileName = "Report.xlsx";
                var startPath = "";
                using (var stream = OpenFile(templateFileName, rootPath, out startPath))
                {
                    package.Load(stream);
                    var ws = package.Workbook.Worksheets[0];

                    int currentRow = 2;

                    foreach (var row in rows)
                    {
                        ws.Cells[currentRow, 1].Value = row.Source;
                        ws.Cells[currentRow, 2].Value = row.FIO;
                        ws.Cells[currentRow, 3].Style.Numberformat.Format = "dd.mm.yyyy hh:MM:ss";
                        ws.Cells[currentRow, 3].Value = row.Date;
                        ws.Cells[currentRow, 4].Value = row.TabNo;
                        ws.Cells[currentRow, 5].Value = row.Context;
                        ws.Cells[currentRow, 6].Value = row.Question;
                        ws.Cells[currentRow, 7].Value = row.OriginalQuestion;
                        ws.Cells[currentRow, 8].Value = row.IsAnswered ? "Да" : "";
                        ws.Cells[currentRow, 9].Value = row.Answer;
                        ws.Cells[currentRow, 10].Value = row.Partition;
                        ws.Cells[currentRow, 11].Value = row.SubPartition;
                        ws.Cells[currentRow, 12].Value = row.Like == 0 ? "" : row.Like.ToString();
                        ws.Cells[currentRow, 13].Value = ClearFromTags(row.AnswerText);
                        ws.Cells[currentRow, 14].Value = row.AnswerText;
                        ws.Cells[currentRow, 15].Value = row.MtoThresholds;
                        ws.Cells[currentRow, 16].Value = row.CategoryOriginId.ToString();
                        ws.Cells[currentRow, 17].Value = (row.IsMto ?? false) ? "МТО" : "";

                        currentRow++;
                    }

                    ws = package.Workbook.Worksheets[1];

                    currentRow = 2;

                    foreach (var row in rows2)
                    {
                        ws.Cells[currentRow, 1].Value = row.Question;
                        ws.Cells[currentRow, 2].Value = row.Count;
                        ws.Cells[currentRow, 3].Value = row.IsAnswered;
                        ws.Cells[currentRow, 4].Value = row.Like;

                        currentRow++;
                    }


                    var ms = new MemoryStream(package.GetAsByteArray());
                    return ms.ToArray();

                }
            }

        }


        private string ClearFromTags(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return "";
            var res = Regex.Replace(txt, @"\<(.*?)\>", " ", RegexOptions.Singleline);
            res = HttpUtility.HtmlDecode(res);
            return res;
        }

        public byte[] MakeReportMtoCompare(List<ReportMtoDto> rows01, int cntChanged, TimeSpan time0, TimeSpan time1, string rootPath)
        {
            using (var package = new ExcelPackage())
            {
                var templateFileName = "ReportMtoCompare.xlsx";
                var startPath = "";
                using (var stream = OpenFile(templateFileName, rootPath, out startPath))
                {
                    package.Load(stream);
                    var ws = package.Workbook.Worksheets[0];

                    ws.Cells[1, 2].Value = rows01.Count;
                    ws.Cells[2, 2].Value = cntChanged;
                    ws.Cells[1, 5].Value = time0;
                    ws.Cells[2, 5].Value = time1;

                    int currentRow = 5;

                    foreach (var row in rows01)
                    {
                        ws.Cells[currentRow, 1].Value = row.Question;
                        if (row.OriginalCategorysElastic != null)
                        {
                            var ids = row.OriginalCategorysElastic.Select(x => x.Id.ToString()).ToList();
                            ws.Cells[currentRow, 2].Value = string.Join("; ", ids);
                            var names = row.OriginalCategorysElastic.Select(x => x.Title).ToList();
                            ws.Cells[currentRow, 3].Value = string.Join("; ", names);
                        }
                        ws.Cells[currentRow, 4].Value = (row.IsMtoAnswer ? "МТО" : "") + " " + (row.IsChanged ? "!!!" : "");
                        if (row.OriginalCategorys != null)
                        {
                            var ids = row.OriginalCategorys.Select(x => x.Id.ToString()).ToList();
                            ws.Cells[currentRow, 5].Value = string.Join("; ", ids);
                            var names = row.OriginalCategorys.Select(x => x.Title).ToList();
                            ws.Cells[currentRow, 6].Value = string.Join("; ", names);
                        }
                        ws.Cells[currentRow, 7].Value = row.ModelResponse;

                        currentRow++;
                    }


                    var ms = new MemoryStream(package.GetAsByteArray());
                    return ms.ToArray();

                }
            }

        }

        public byte[] MakeReportMto(List<ReportSpellDto> rows01, int cntAll, TimeSpan time0, TimeSpan time1, string rootPath)
        {
            using (var package = new ExcelPackage())
            {
                var templateFileName = "ReportMto.xlsx";
                var startPath = "";
                using (var stream = OpenFile(templateFileName, rootPath, out startPath))
                {
                    package.Load(stream);
                    var ws = package.Workbook.Worksheets[0];

                    ws.Cells[1, 2].Value = cntAll;
                    ws.Cells[2, 2].Value = rows01.Count;
                    ws.Cells[1, 5].Value = time0;
                    ws.Cells[2, 5].Value = time1;

                    int currentRow = 5;

                    foreach (var row in rows01)
                    {
                        ws.Cells[currentRow, 1].Value = row.OriginalQuestion;
                        ws.Cells[currentRow, 2].Value = row.QuestionFirst;
                        //ws.Cells[currentRow, 3].Value = row.QuestionSecond;
                        ws.Cells[currentRow, 3].Value = row.AnswerFirst;
                        ws.Cells[currentRow, 4].Value = row.AnswerSecond;
                        ws.Cells[currentRow, 5].Value = (row.IsMto ?? false) ? "МТО" : "";

                        currentRow++;
                    }



                    var ms = new MemoryStream(package.GetAsByteArray());
                    return ms.ToArray();

                }
            }

        }

        public byte[] MakeReportMtoAnswers(List<ReportMtoDto> rows01, string rootPath)
        {
            using (var package = new ExcelPackage())
            {
                var templateFileName = "ReportMtoAnswers.xlsx";
                var startPath = "";
                using (var stream = OpenFile(templateFileName, rootPath, out startPath))
                {
                    package.Load(stream);
                    var ws = package.Workbook.Worksheets[0];

                    int currentRow = 2;

                    foreach (var row in rows01)
                    {
                        ws.Cells[currentRow, 1].Value = row.Question;
                        ws.Cells[currentRow, 2].Value = row.IsMtoAnswer ? "МТО" : "";
                        if (row.OriginalCategorys != null)
                        {
                            var ids = row.OriginalCategorys.Select(x => x.Id.ToString()).ToList();
                            ws.Cells[currentRow, 3].Value = string.Join("; ", ids);
                            var names = row.OriginalCategorys.Select(x => x.Title).ToList();
                            ws.Cells[currentRow, 4].Value = string.Join("; ", names);
                        }
                        ws.Cells[currentRow, 5].Value = row.ModelResponse;

                        currentRow++;
                    }



                    var ms = new MemoryStream(package.GetAsByteArray());
                    return ms.ToArray();

                }
            }

        }

    }
}
