using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Contexts.ChatBot
{
    class ModelLearningConf : EntityInt
    {
        public Guid ModelLearningId { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? ToCategoryId { get; set; }
        public string ToCategoryName { get; set; }
        public double? Confusion { get; set; }
    }
}
