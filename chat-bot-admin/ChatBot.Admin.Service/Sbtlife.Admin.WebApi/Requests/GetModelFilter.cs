using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.WebApi.Requests
{
    public class GetModelFilter
    {
        public string Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public SortingDto[] Sorting { get; set; }
        public FilteringDto[] Filtering { get; set; }
    }
}
