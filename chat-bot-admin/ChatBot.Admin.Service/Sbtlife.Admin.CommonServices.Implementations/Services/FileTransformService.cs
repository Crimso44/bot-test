using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LingvoNET;
using OfficeOpenXml;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommonServices.Services
{
    class FileTransformService : IFileTransformService
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

        public byte[] MakeXls(CategoryDto[] rows, string rootPath)
        {
            using (var package = new ExcelPackage())
            {
                var templateFileName = "SBoT.xlsx";
                var startPath = "";
                using (var stream = OpenFile(templateFileName, rootPath, out startPath))
                {
                    package.Load(stream);
                    var ws = package.Workbook.Worksheets[1];

                    int currentRow = 2;

                    foreach (var row in rows)
                    {
                        ws.Cells[currentRow, 1].Value = row.Name;
                        ws.Cells[currentRow, 2].Value = row.SetContext;
                        ws.Cells[currentRow, 6].Value = row.Response;
                        if (row.Partition != null)
                            ws.Cells[currentRow, 11].Value = row.Partition.Title;
                        if (row.UpperPartition != null)
                            ws.Cells[currentRow, 12].Value = row.UpperPartition.Title;
                        ws.Cells[currentRow, 13].Value = row.OriginId.ToString();

                        currentRow++;
                        var learnCurRow = currentRow;

                        foreach (var p in row.Patterns)
                        {
                            if (p == null) continue;

                            ws.Cells[currentRow, 3].Value = p.Context;
                            ws.Cells[currentRow, 4].Value = p.OnlyContext ?? false ? "+" : "";
                            ws.Cells[currentRow, 7].Value = p.Phrase;

                            if (p.Words.Any())
                            {
                                foreach (var w in p.Words)
                                {
                                    ws.Cells[currentRow, 8].Value = w.WordName;
                                    if (w.WordTypeId.HasValue)
                                    {
                                        var x = "";
                                        switch (w.WordTypeId.Value)
                                        {
                                            case (int)Analyser.WordType.Noun:
                                                x = "N";
                                                break;
                                            case (int)Analyser.WordType.Adjective:
                                                x = "A";
                                                break;
                                            case (int)Analyser.WordType.Verb:
                                                x = "V";
                                                break;
                                        }
                                        ws.Cells[currentRow, 9].Value = x;
                                    }

                                    currentRow++;
                                }
                            }
                            else
                            {
                                currentRow++;
                            }
                        }
                        
                        if (row.Learnings != null) {
                            foreach (var l in row.Learnings)
                            {
                                ws.Cells[learnCurRow, 10].Value = l.Question;
                                learnCurRow++;
                            }
                        }

                        currentRow = Math.Max(learnCurRow, currentRow);
                    }

                    var ms = new MemoryStream(package.GetAsByteArray());
                    return ms.ToArray();

                }
            }
        }
    }
}
