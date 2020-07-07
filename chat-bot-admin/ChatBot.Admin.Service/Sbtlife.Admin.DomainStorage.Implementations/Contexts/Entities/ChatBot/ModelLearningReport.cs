using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class ModelLearningReport : EntityInt
    {
        public Guid ModelLearningId { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Markup { get; set; }
        public double? Accuracy { get; set; }
        public double? Precision { get; set; }
        public double? Recall { get; set; }
        public double? F1 { get; set; }
    }
}
