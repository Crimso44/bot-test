using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class ModelLearningMarkup : EntityInt
    {
        public Guid ModelLearningId { get; set; }
        public Guid? CategoryFrom { get; set; }
        public Guid? CategoryTo { get; set; }
        public string Question { get; set; }
    }
}
