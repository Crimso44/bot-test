using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class ModelLearningMarkup : EntityInt
    {
        public Guid ModelLearningId { get; set; }
        public Guid? CategoryFrom { get; set; }
        public Guid? CategoryTo { get; set; }
        public string Question { get; set; }
        public string CategoryName { get; set; }
        public string ToCategoryName { get; set; }
    }
}
