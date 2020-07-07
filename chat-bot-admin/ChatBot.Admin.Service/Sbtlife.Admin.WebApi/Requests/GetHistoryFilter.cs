using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.WebApi.Requests
{
    public class GetHistoryFilter
    {
        public string Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public SortingDto[] Sorting { get; set; }
        public FilteringDto[] Filtering { get; set; }

        public bool? IsAnsweredMl { get; set; }
        public bool? IsAnsweredEve { get; set; }
        public bool? IsAnsweredButton { get; set; }
        public bool? IsAnsweredNo { get; set; }
        public bool? IsAnsweredOther { get; set; }

        public bool? IsLikeYes { get; set; }
        public bool? IsLikeNo { get; set; }
        public bool? IsDisLike { get; set; }

        public bool? IsMlYes { get; set; }
        public bool? IsMlAnswer { get; set; }
        public bool? IsMlWrong { get; set; }
        public bool? IsMlNo { get; set; }

        public Guid? CategoryOriginId { get; set; }
    }
}
