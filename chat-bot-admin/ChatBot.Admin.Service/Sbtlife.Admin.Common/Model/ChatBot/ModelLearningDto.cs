using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    public class ModelLearningDto
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Command { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int? Markup { get; set; }
        public double? Accuracy { get; set; }
        public double? Precision { get; set; }
        public double? Recall { get; set; }
        public double? F1 { get; set; }
        public Guid? ModelDocumentId { get; set; }
        public bool IsActive { get; set; }
        public List<ModelReportDto> Report { get; set; }
    }

    public class ModelReportDto
    {
        public Guid Id { get; set; }
        public Guid ModelLearningId { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? PartitionId { get; set; }
        public Guid? UpperPartitionId { get; set; }
        public int? Markup { get; set; }
        public double? Accuracy { get; set; }
        public double? Precision { get; set; }
        public double? Recall { get; set; }
        public double? F1 { get; set; }

        public List<ModelReportConfusionDto> ConfusionFrom { get; set; }
        public List<ModelReportConfusionDto> ConfusionTo { get; set; }
    }

    public class ModelReportConfusionDto
    {
        public Guid? OriginId { get; set; }
        public string CategoryName { get; set; }
        public int? Confusion { get; set; }
        public int? CategoryId { get; set; }
        public List<string> Questions { get; set; }
    }
}