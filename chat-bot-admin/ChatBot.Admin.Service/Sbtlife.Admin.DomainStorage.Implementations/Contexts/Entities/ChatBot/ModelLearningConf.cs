using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class ModelLearningConf : EntityInt
    {
        public Guid ModelLearningId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ToCategoryId { get; set; }
        public double? Confusion { get; set; }
    }
}
