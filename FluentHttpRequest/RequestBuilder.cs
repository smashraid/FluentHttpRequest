using System;
using System.Collections.Specialized;
using FluentHttpRequest.Helpers;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FluentHttpRequest
{
    public class RequestBuilder
    {
        private Uri _endpoint;

        private NameValueCollection _parameters;

        private NameValueCollection _requestHeaders;

        private Type _type;

        private HttpMethod _method;

        private string _response;


        private RequestBuilder()
        {
            this._type = typeof(string);

            this._parameters = HttpUtility.ParseQueryString(string.Empty);

            this._requestHeaders = new  NameValueCollection();

            this._method = HttpMethod.GET;
        }
        public static RequestBuilder Create(string endpoint)
        {
            return new RequestBuilder() { _endpoint = new Uri(endpoint) };
        }
        public RequestBuilder Execute()
        {
            switch (this._method)
            {
                case HttpMethod.GET:
                    this._response = Http.Get(this._endpoint, this._parameters, this._requestHeaders);
                    break;

                case HttpMethod.POST:
                    this._response = Http.Post(this._endpoint, _parameters, this._requestHeaders);
                    break;

                default:
                    this._response = Http.Get(this._endpoint, this._parameters, this._requestHeaders);
                    break;
            }

            return this;
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
        public object Fill<T>()
        {
            return JsonConvert.DeserializeObject<T>(this._response);
        }
        public RequestBuilder Extract(string path)
        {
            JToken jsonResponse = JToken.Parse(this._response);

            this._response = jsonResponse.SelectToken(path).ToString();

            return this;
        }
    }
}
