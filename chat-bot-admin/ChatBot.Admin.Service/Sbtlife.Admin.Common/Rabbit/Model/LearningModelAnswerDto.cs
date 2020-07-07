using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatBot.Admin.Common.Rabbit.Model
{
    [DataContract]
    public class LearningModelAnswerDto
    {
        [DataMember]
        public int Markup { get; set; }
        [DataMember]
        public LearningModelMetricsDto Quality_metrics { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string[][] Conf_matrix { get; set; }
        [DataMember]
        public string[] Conf_matrix_labels { get; set; }
        [DataMember]
        public LearningModelMarkupDto Conf_markup { get; set; }
    }

    [DataContract]
    public class LearningModelMarkupDto
    {
        [DataMember]
        public string[] Question_lemmas { get; set; }
        [DataMember]
        public string[] Category_num { get; set; }
        [DataMember]
        public string[] Answer_id { get; set; }
        [DataMember]
        public string[] Answer_id_predicted { get; set; }
    }

    [DataContract]
    public class LearningModelMetricsDto
    {
        [DataMember]
        public float Accuracy { get; set; }
        [DataMember]
        public float Precision_macro_average { get; set; }
        [DataMember]
        public float Recall_macro_average { get; set; }
        [DataMember]
        public float Macro_f1 { get; set; }
        [DataMember]
        public string[][] Classes_report { get; set; }
    }
}
