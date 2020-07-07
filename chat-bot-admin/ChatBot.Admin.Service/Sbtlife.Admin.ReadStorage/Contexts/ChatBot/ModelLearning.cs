using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class ModelLearning : Entity
    {
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
    }
}
