using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace Eve
{

    public class EveWebRequest
    {
        public event EventHandler<AnswerDtoReceivedEventArgs> AnswerReceived;
        public event EventHandler<AnswerDtoReceivedEventArgs> HiddenAnswerReceived;
        public event EventHandler<RosterDtoReceivedEventArgs> RosterReceived;
        public event EventHandler<StringReceivedEventArgs> StringReceived;
        public event EventHandler<HistoryReceivedEventArgs> HistoryReceived;

        public void PostQuestion(string url, RequestDto parameters, bool showAnswer)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoPostQuestion;
            if (showAnswer)
                worker.RunWorkerCompleted += AnswerLoaded;
            else
                worker.RunWorkerCompleted += HiddenAnswerLoaded;
            worker.RunWorkerAsync(new WebRequestDto() { Url = url, Parameters = parameters });
        }

        public void PostData(string url, object parameters)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoPost;
            worker.RunWorkerAsync(new WebRequestDto() { Url = url, Parameters = parameters });
        }

        public void PostRosterData(string url, FindRequestDto parameters)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoPostRosterData;
            worker.RunWorkerCompleted += RosterLoaded;
            worker.RunWorkerAsync(new WebRequestDto() { Url = url, Parameters = parameters });
        }

        public void GetString(string url, RequestDto parameters)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoGetString;
            worker.RunWorkerCompleted += StringLoaded;
            worker.RunWorkerAsync(new WebRequestDto() { Url = url, Parameters = parameters });
        }

        public void GetHistory(string url, int historyId)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoGetHistory;
            worker.RunWorkerCompleted += HistoryLoaded;
            worker.RunWorkerAsync(new WebRequestDto() { Url = $"{url}/{historyId}", Parameters = null });
        }

        private void DoPostQuestion(object sender, DoWorkEventArgs e)
        {
            var data = (WebRequestDto)e.Argument;
            e.Result = WebApiRequest<AnswerDto>(data.Url, data.Parameters, WebRequestMethods.Http.Post);
        }

        private void DoPost(object sender, DoWorkEventArgs e)
        {
            var data = (WebRequestDto)e.Argument;
            e.Result = WebApiRequest<object>(data.Url, data.Parameters, WebRequestMethods.Http.Post);
        }

        private void DoPostRosterData(object sender, DoWorkEventArgs e)
        {
            var data = (WebRequestDto)e.Argument;
            var res = WebApiRequest<List<RosterDto>>(data.Url, data.Parameters, WebRequestMethods.Http.Post);
            e.Result = new RosterDtoReceivedEventArgs()
            {
                Request = (FindRequestDto)data.Parameters,
                Roster = res
            };
        }

        private void DoGetString(object sender, DoWorkEventArgs e)
        {
            var data = (WebRequestDto)e.Argument;
            e.Result = WebApiRequest<string>(data.Url, data.Parameters, WebRequestMethods.Http.Get);
        }

        private void DoGetHistory(object sender, DoWorkEventArgs e)
        {
            var data = (WebRequestDto)e.Argument;
            e.Result = WebApiRequest<List<HistoryDto>>(data.Url, data.Parameters, WebRequestMethods.Http.Get);
        }

        private void AnswerLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                AnswerReceived?.Invoke(sender, new AnswerDtoReceivedEventArgs() { Answer = (AnswerDto)e.Result });
            } else
            {
                AnswerReceived?.Invoke(sender, new AnswerDtoReceivedEventArgs() { Answer = new AnswerDto() { Title = "Что-то пошло не так :(" } });
            }
        }

        private void HiddenAnswerLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                HiddenAnswerReceived?.Invoke(sender, new AnswerDtoReceivedEventArgs() { Answer = (AnswerDto)e.Result });
            }
            else
            {
                HiddenAnswerReceived?.Invoke(sender, new AnswerDtoReceivedEventArgs() { Answer = new AnswerDto() { Title = "Что-то пошло не так :(" } });
            }
        }

        private void RosterLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                RosterReceived?.Invoke(sender, (RosterDtoReceivedEventArgs)e.Result);
            }
            else
            {
                RosterReceived?.Invoke(sender, new RosterDtoReceivedEventArgs() { Roster = new List<RosterDto>() });
            }
        }

        private void StringLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                StringReceived?.Invoke(sender, new StringReceivedEventArgs() { StringAnswer = (string)e.Result });
            }
            else
            {
                StringReceived?.Invoke(sender, new StringReceivedEventArgs() { StringAnswer = "" });
            }
        }

        private void HistoryLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                HistoryReceived?.Invoke(sender, new HistoryReceivedEventArgs() { HistoryAnswer = (List<HistoryDto>)e.Result });
            }
            else
            {
                HistoryReceived?.Invoke(sender, new HistoryReceivedEventArgs() { HistoryAnswer = new List<HistoryDto>() });
            }
        }

        private T WebApiRequest<T>(string url, object requestData, string method)
        {
            var webRequest = GetRequest(url, method);

            if (requestData != null)
            {
                using (var rs = webRequest.GetRequestStream())
                {
                    using (var streamWriter = new StreamWriter(rs))
                    {
                        var json = JsonConvert.SerializeObject(requestData);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
                }
            }

            return GetGetResponse<T>(webRequest);
        }


        private static HttpWebRequest GetRequest(string url, string method)
        {
            if (!(WebRequest.Create(url) is HttpWebRequest webRequest))
            {
                throw new Exception("Не могу открыть URL");
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

        private static T GetGetResponse<T>(WebRequest webRequest)
        {
            var result = default(T);
            using (var response = webRequest.GetResponse() as HttpWebResponse)
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
