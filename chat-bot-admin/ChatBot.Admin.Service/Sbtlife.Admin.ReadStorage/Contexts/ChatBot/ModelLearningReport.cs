using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class ModelLearningReport : EntityInt
    {
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
    }
}
