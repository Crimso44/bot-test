using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Repository.Interfaces;

namespace SBoT.Code.Repository
{
    public class UsefulLinksRepository : IUsefulLinksRepository
    {
        private readonly IOptions<Urls> _urls;
        private readonly IWebRequestProcess _request;

        public UsefulLinksRepository(IOptions<Urls> urls, IWebRequestProcess request)
        {
            _urls = urls;
            _request = request;
        }


        public List<LinkDto> SearchLinks(string question)
        {
            var res = _request.WebApiRequestGet<List<LinkDto>>($"{_urls.Value.ChatInfo}/info/find", new Dictionary<string, object> { { "question", question } });
            return res;
        }

        public string FormatLinks(List<LinkDto> links, int limit) { 
            var res = "";
            var i = 0;
            while (i < limit && i < links.Count)
            {
                res += MakeLink(links[i]);
                i++;
            }

            if (i == limit)
            {
                if (links.Count == limit + 1)
                    res += MakeLink(links[5]);
                else if (links.Count > limit + 1)
                    res += "и другие...";
            }

            return res;
        }

        private static string MakeLink(LinkDto link)
        {
            var res = $"<a href='{link.Url}' target='_blank'>{link.Name}</a><br/>";
            return res;
        }

    }
}
