using System;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using FluentHttpRequest.LifecycleManagement;
using FluentHttpRequest.CacheExtension;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Security.Cryptography;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace FluentHttpRequest
{
    public class RequestBuilder : IFluentOperation, IFluentEnviroment, IFluentEndpoint, IFluentSecurity
    {
        private Uri _uri;
        private NameValueCollection _parameters;
        private NameValueCollection _requestHeaders;
        private NameValueCollection _bodyParameters;
        private Type _type;
        private string _response;
        private X509Certificate2 _certificate;

        private string _project;
        private string _enviroment;
        private string _endpoint;

        private Utils _utils;

        private RequestBuilder()
        {
            _type = typeof(string);
            _parameters = HttpUtility.ParseQueryString(string.Empty);
            _requestHeaders = new NameValueCollection();
            _bodyParameters = new NameValueCollection();
            _utils = new Utils();
        }

        public static IFluentOperation Create(string url)
        {
            return new RequestBuilder() { _uri = new Uri(url) };
        }
        public static IFluentEnviroment Project(string name)
        {
            return new RequestBuilder() { _project = name };
        }
        public IFluentEndpoint Env(string enviroment)
        {
            _enviroment = enviroment;
            return this;
        }
        public IFluentSecurity Endpoint(string endpoint)
        {
            _endpoint = endpoint;
            string u = $"https://lm.cignium.com/run/cignium/{_project}/{_enviroment}/{_endpoint}";
            //return new RequestBuilder() { _uri = new Uri(u) };
            _uri = new Uri(u);
            return this;
        }
        public IFluentOperation AddParam(string param, string value)
        {
            _parameters.Add(param, value);
            return this;
        }
        public IFluentOperation AddHeader(string header, string value)
        {
            _requestHeaders.Add(header, value);
            return this;
        }
        public IFluentOperation AddBodyParam(string param, string value)
        {
            _bodyParameters.Add(param, value);
            return this;
        }
        public IFluentOperation AddCertificate(string name, StoreName storeName = StoreName.Root, StoreLocation storeLocation = StoreLocation.CurrentUser)
        {
            X509Store x509CertStore = new X509Store(storeName, storeLocation);
            x509CertStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificate2Collection = x509CertStore.Certificates.Find(X509FindType.FindBySubjectName, name, true);
            _certificate = certificate2Collection[0];
            x509CertStore.Close();
            return this;
        }
        public IFluentOperation AddCertificate(string name, string password)
        {
            _certificate = new X509Certificate2(name, password);
            return this;
        }
        IFluentOperation IFluentSecurity.AddSecurityKey(string key, string secret)
        {
            string hash = string.Empty;
            string currentUtcDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret.ToUpper())))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n", "GET", currentUtcDate, _uri.AbsolutePath.ToLower())));
                hash = Convert.ToBase64String(computedHash);
            }
            _requestHeaders.Add("Timestamp", currentUtcDate);
            _requestHeaders.Add(HttpRequestHeader.Authorization.ToString(), "API-KEY" + " " + key + ":" + hash);
            return this;
        }
        public string GetQueryString(bool printPort = false)
        {
            var uriBuilder = new UriBuilder(_uri) { Query = _parameters.ToString() };
            if (!printPort) uriBuilder.Port = -1;
            return uriBuilder.ToString();
        }

        public T Get<T>(string path = null)
        {
            string response = Request(HttpMethod.Get, GetQueryString(), _requestHeaders, null, _certificate);
            if (!string.IsNullOrEmpty(path))
            {
                response = _utils.Extract(response, path);
            }
            T result = _utils.Fill<T>(response);
            return result;
        }

        public async Task<T> GetAsync<T>(string path = null)
        {
            return await GetAsync<T>(CancellationToken.None, path);
        }
        public async Task<T> GetAsync<T>(CancellationToken cancellationToken, string path = null)
        {
            string response = await RequestAsync(System.Net.Http.HttpMethod.Get, cancellationToken);
            if (!string.IsNullOrEmpty(path))
            {
                response = _utils.Extract(response, path);
            }
            T result = _utils.Fill<T>(response);
            return result;
        }

        public T Post<T>(string path = null)
        {
            string response = Request(HttpMethod.Post, GetQueryString(), _requestHeaders, _bodyParameters, _certificate);
            if (!string.IsNullOrEmpty(path))
            {
                response = _utils.Extract(response, path);
            }
            T result = _utils.Fill<T>(response);
            return result;
        }

        public async Task<T> PostAsync<T>(string path = null)
        {
            return await PostAsync<T>(CancellationToken.None, path);
        }

        public async Task<T> PostAsync<T>(CancellationToken cancellationToken, string path = null)
        {
            string response = await RequestAsync(System.Net.Http.HttpMethod.Post, cancellationToken);
            if (!string.IsNullOrEmpty(path))
            {
                response = _utils.Extract(response, path);
            }
            T result = _utils.Fill<T>(response);
            return result;
        }

        #region Helper

        internal string Request(
     System.Net.Http.HttpMethod method,
     string url,
     NameValueCollection headers = null,
     NameValueCollection bodyParameters = null,
     X509Certificate2 certificate = null)
        {
            string response = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = method.Method;
            if (certificate != null) { httpWebRequest.ClientCertificates.Add(certificate); }
            if (headers != null && headers.Count > 0) { httpWebRequest.Headers.Add(headers); }
            if (bodyParameters != null && bodyParameters.Count > 0)
            {
                byte[] data = Encoding.ASCII.GetBytes(bodyParameters.GetPostData());
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = data.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (Stream dataStream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        response = reader.ReadToEnd();
                    }
                }
            }

            return response;
        }

        private async Task<string> RequestAsync(System.Net.Http.HttpMethod method, CancellationToken cancellationToken)
        {
            string result = string.Empty;
            using (WebRequestHandler handler = new WebRequestHandler())
            {
                if (_certificate != null)
                {
                    handler.ClientCertificates.Add(_certificate);
                }

                using (HttpClient client = new HttpClient(handler))
                {
                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(GetQueryString()),
                        Method = method
                    };

                    foreach (string key in _requestHeaders.Keys)
                    {
                        httpRequestMessage.Headers.Add(key, _requestHeaders[key]);
                    }

                    if (_bodyParameters.Count > 0)
                    {
                        httpRequestMessage.Content = new FormUrlEncodedContent((_bodyParameters.ToKeyValuePairCollection()));
                    }

                    using (HttpResponseMessage response = await client.SendAsync(httpRequestMessage, cancellationToken))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                result = await content.ReadAsStringAsync();
                            }
                        }
                    }
                }

            }
            return result;
        }


        #endregion
    }

    public class Utils
    {
        public string Extract(string response, string path)
        {
            JToken jsonResponse = JToken.Parse(response);
            return jsonResponse.SelectToken(path).ToString();
        }
        public T Fill<T>(string response)
        {
            T t = default(T);
            Type type = typeof(T);
            if (!string.IsNullOrEmpty(response) 
                && type != typeof(byte) 
                && type != typeof(sbyte) 
                && type != typeof(int)
                && type != typeof(uint)
                && type != typeof(short)
                && type != typeof(ushort)
                && type != typeof(long)
                && type != typeof(ulong)
                && type != typeof(float)
                && type != typeof(double)
                && type != typeof(char)
                && type != typeof(bool)
                && type != typeof(string)
                && type != typeof(decimal)
                )
            {
                t = JsonConvert.DeserializeObject<T>(response);
            }
            else
            {
                t = (T)Convert.ChangeType(response, type);
            }
            return t;
        }
        public T WithCache<T>(T t, string key, bool withFallback = false)
        {
            Type type = t.GetType();
            string region = type.BaseType.Name;
            if (type.GetInterface("IEnumerable") != null)
            {
                IEnumerable<object> list = (IEnumerable<object>)t;
                Cache.Storage.AddRange(list, key, region);
            }
            else
            {
                object value = t.GetType().GetProperty(key).GetValue(t);
                Cache.Storage.Add(t, value.ToString(), region);
            }
            if (withFallback)
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}/{region}.cache";
                Write(path, JsonConvert.SerializeObject(t));
            }
            return t;
        }
        public WebRequestHandler AddCertificate(X509Certificate certificate)
        {
            WebRequestHandler handler = new WebRequestHandler();
            handler.ClientCertificates.Add(certificate);
            return handler;
        }
        public HttpRequestMessage BuildRequestMessage(System.Net.Http.HttpMethod method, string url, NameValueCollection headers, NameValueCollection bodyParameters)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = method
            };

            foreach (string key in headers.Keys)
            {
                httpRequestMessage.Headers.Add(key, headers[key]);
            }

            if (bodyParameters.Count > 0)
            {
                httpRequestMessage.Content = new FormUrlEncodedContent(bodyParameters.ToKeyValuePairCollection());
            }

            return httpRequestMessage;
        }
        public void Write(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
        public async void WriteAsync(string path, string contents)
        {
            byte[] result = Encoding.ASCII.GetBytes(contents);
            using (FileStream sourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                sourceStream.Seek(0, SeekOrigin.End);
                await sourceStream.WriteAsync(result, 0, result.Length);
            }
        }
        public async Task<string> ReadAsync(string path)
        {
            byte[] result;
            string text = string.Empty;
            using (FileStream sourceStream = File.Open(path, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
                text = Encoding.ASCII.GetString(result);
            }
            return text;
        }
        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }

    public static class RequestBuilderExtension
    {
        public static T WithCache<T>(this T t, string key)
        {
            Type type = t.GetType();
            string region = type.BaseType.Name;
            if (type.GetInterface("IEnumerable") != null)
            {
                IEnumerable<object> list = (IEnumerable<object>)t;
                Cache.Storage.AddRange(list, key, region);
            }
            else
            {
                object value = t.GetType().GetProperty(key).GetValue(t);
                Cache.Storage.Add(t, value.ToString(), region);
            }
            return t;
        }

        public static T AndFallback<T>(this T t)
        {
            Utils utils = new Utils();
            Type type = t.GetType();
            string region = type.BaseType.Name;
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/{region}.cache";
            utils.Write(path, JsonConvert.SerializeObject(t));
            return t;
        }

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairCollection(this NameValueCollection nameValueCollection)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (string key in nameValueCollection.Keys)
            {
                list.Add(new KeyValuePair<string, string>(key, nameValueCollection[key]));
            }
            return list;
        }

        public static string GetPostData(this NameValueCollection nameValueCollection)
        {
            string postData = string.Empty;

            foreach (string key in nameValueCollection.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(nameValueCollection[key]) + "&";
            }
            return postData;
        }
    }


}
