using System;
using System.Collections.Specialized;
using FluentHttpRequest.Helpers;
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
using FluentHttpRequest.FileExtension;

namespace FluentHttpRequest
{
    public class RequestBuilder : IFluentOperation, IFluentProcess, IFluentTransform, IFluentEnviroment, IFluentEndpoint, IFluentSecurity
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

        private RequestBuilder()
        {
            _type = typeof(string);

            _parameters = HttpUtility.ParseQueryString(string.Empty);

            _requestHeaders = new NameValueCollection();

            _bodyParameters = new NameValueCollection();
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

        public IFluentProcess Get()
        {
            _response = Http.Get(GetQueryString(), _requestHeaders, _certificate);
            return this;
        }

        public IFluentProcess Post()
        {
            _response = Http.Post(GetQueryString(), _requestHeaders, _bodyParameters, _certificate);
            return this;
        }

        public Task<IFluentProcess> GetAsync()
        {
            return Task.Run(() =>
            {
                return Get();
            });
        }

        public Task<IFluentProcess> GetAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return Get();
            }, cancellationToken);
        }

        public Task<IFluentProcess> PostAsync()
        {
            return Task.Run(() =>
            {
                return Post();
            });
        }

        public Task<IFluentProcess> PostAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return Post();
            }, cancellationToken);
        }
        public string GetQueryString(bool printPort = false)
        {
            var uriBuilder = new UriBuilder(_uri) { Query = _parameters.ToString() };
            if (!printPort) uriBuilder.Port = -1;
            return uriBuilder.ToString();
        }

        public IFluentTransform Extract(string path)
        {
            JToken jsonResponse = JToken.Parse(_response);
            _response = jsonResponse.SelectToken(path).ToString();
            return this;
        }

        public T Fill<T>()
        {
            return JsonConvert.DeserializeObject<T>(_response);
        }

        public T FillWithCache<T>(string key, string region, bool withFallback = false)
        {
            T t = Fill<T>();
            if (t.GetType().GetInterface("IEnumerable") != null)
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
                FileBuilder.Flat.Write(path, JsonConvert.SerializeObject(t));
            }
            return t;
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

    }
}
