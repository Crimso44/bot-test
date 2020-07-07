using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class History : EntityInt
    {
        public DateTime QuestionDate { get; set; }

        public string SigmaLogin { get; set; }
        public string UserName { get; set; }
        public string Question { get; set; }
        public string OriginalQuestion { get; set; }
        public string Answer { get; set; }
        public string AnswerText { get; set; }
        public string AnswerType { get; set; }

        public decimal? Rate { get; set; }

        public string SetContext { get; set; }
        public string Context { get; set; }

        public bool IsButton { get; set; }
        public System.Int16? Like { get; set; }
        public Guid? CategoryOriginId { get; set; }
        public bool? IsMto { get; set; }
        public string MtoThresholds { get; set; }

        public int? LearnId { get; set; }
        public Guid? LearnCategoryId { get; set; }
        public bool AnswerGood { get; set; }

        public string Source { get; set; }
    }
}
