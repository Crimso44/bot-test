using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SBoT.Code.Entity.Interfaces;

namespace SBoT.Code.Entity
{
    internal class WebRequestProcess : IWebRequestProcess
    {
        private readonly string _proxyDomain;
        private readonly int _proxyPort;

        public WebRequestProcess()
        {
            _proxyDomain = "";
            _proxyPort = 0;
        }

        public T WebApiRequestPost<T>(string url, object requestData)
        {
            return WebApiRequestPostAsync<T>(url, requestData).GetAwaiter().GetResult();
        }

        public T WebApiRequestGet<T>(string url, Dictionary<string, object> data = null)
        {
            return WebApiRequestGetAsync<T>(url, data).GetAwaiter().GetResult();
        }

        public async Task<T> WebApiRequestPostAsync<T>(string url, object requestData)
        {
            var webRequest = GetRequest(url, WebRequestMethods.Http.Post);

            if (requestData != null)
            {
                using (var rs = await webRequest.GetRequestStreamAsync())
                {
                    using (var streamWriter = new StreamWriter(rs))
                    {
                        var json = JsonConvert.SerializeObject(requestData);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
                }
            }

            return await GetGetResponse<T>(webRequest);
        }

        public async Task<T> WebApiRequestGetAsync<T>(string url, Dictionary<string, object> data = null)
        {
            var webRequest = GetRequest(url + (data == null
                                            ? null
                                            : "?" + string.Join("&", data.Select(x => x.Key + "=" + x.Value).ToArray())), WebRequestMethods.Http.Get);
            return await GetGetResponse<T>(webRequest);
        }

        private HttpWebRequest GetRequest(string url, string method)
        {
            if (!(WebRequest.Create(url) is HttpWebRequest webRequest))
            {
                throw new Exception("Не могу открыть URL");
            }

            if (!string.IsNullOrEmpty(_proxyDomain))
            {
                var webProxy = new WebProxy(_proxyDomain, _proxyPort)
                {
                    BypassProxyOnLocal = true
                };
                webRequest.Proxy = webProxy;
            }

            webRequest.KeepAlive = true;
            webRequest.Method = method;
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Accept = "application/json";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;

            return webRequest;
        }

        private static bool AcceptAllCertifications(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static async Task<T> GetGetResponse<T>(WebRequest webRequest)
        {
            var result = default(T);
            using (var response = await webRequest.GetResponseAsync() as HttpWebResponse)
            {
                if (response != null)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            return default(T);
                        }

                        using (var reader = new StreamReader(stream))
                        {
                            var jstr = reader.ReadToEnd();
                            result = JsonConvert.DeserializeObject<T>(jstr);
                        }
                    }
                }
            }

            return result;
        }
    }
}
