using System;
using System.Collections.Specialized;
using FluentHttpRequest.Helpers;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentHttpRequest
{
    public class RequestBuilder
    {
        private Uri _endpoint;

        private NameValueCollection _parameters;

        private NameValueCollection _requestHeaders;
        private NameValueCollection _bodyParameters;

        private Type _type;

        private HttpMethod _method;

        private string _response;


        private RequestBuilder()
        {
            _type = typeof(string);

            _parameters = HttpUtility.ParseQueryString(string.Empty);

            _requestHeaders = new NameValueCollection();

            _bodyParameters = new NameValueCollection();

            _method = HttpMethod.GET;
        }
        public static RequestBuilder Create(string endpoint)
        {
            return new RequestBuilder() { _endpoint = new Uri(endpoint) };
        }

        public RequestBuilder AddParam(string param, string value)
        {
            this._parameters.Add(param, value);

            return this;
        }

        public RequestBuilder AddParam<T>(T value) where T : class
        {
            this._parameters.Add(value.ToNameCollection());
            return this;
        }

        public RequestBuilder AddParam<T>(List<T> values) where T : class
        {
            values.ForEach(value => this._parameters.Add(value.ToNameCollection()));
            return this;
        }

        public RequestBuilder AddHeader(string header, string value)
        {
            this._requestHeaders.Add(header, value);
            return this;
        }

        public RequestBuilder AddBodyParam(string param, string value)
        {
            this._bodyParameters.Add(param, value);
            return this;
        }

        public string GetQueryString(bool printPort = false)
        {
            var uriBuilder = new UriBuilder(this._endpoint) { Query = this._parameters.ToString() };

            if (!printPort) uriBuilder.Port = -1;

            return uriBuilder.ToString();
        }

        public RequestBuilder Method(HttpMethod method)
        {
            this._method = method;

            return this;
        }

        public T Fill<T>()
        {
            return JsonConvert.DeserializeObject<T>(this._response);
        }

        public RequestBuilder Extract(string path)
        {
            JToken jsonResponse = JToken.Parse(this._response);

            this._response = jsonResponse.SelectToken(path).ToString();

            return this;
        }

        public RequestBuilder Get()
        {
            _response = Http.Get(GetQueryString(), _requestHeaders);
            return this;
        }

        public RequestBuilder Post()
        {
            _response = Http.Post(GetQueryString(), _requestHeaders, _bodyParameters);
            return this;
        }

        public Task<RequestBuilder> GetAsync()
        {
            return Task.Run(() =>
            {
                return Get();
            });
        }

        public Task<RequestBuilder> PostAsync()
        {
            return Task.Run(() =>
            {
                return Get();
            });
        }

    }
}
