using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IWebRequestProcess
    {
        T WebApiRequestPost<T>(string url, object requestData);
        T WebApiRequestGet<T>(string url, Dictionary<string, object> data = null);

        Task<T> WebApiRequestPostAsync<T>(string url, object requestData);
        Task<T> WebApiRequestGetAsync<T>(string url, Dictionary<string, object> data = null);
    }
}
