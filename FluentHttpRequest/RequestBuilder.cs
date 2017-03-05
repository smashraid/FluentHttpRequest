using System;
using System.Collections.Specialized;
using FluentHttpRequest.Helpers;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace FluentHttpRequest
{
    public class RequestBuilder : IFluentHttp, IFluentOperation,  IFluentProcess, IFluentTransform
    {
        private Uri _endpoint;
        private NameValueCollection _parameters;
        private NameValueCollection _requestHeaders;
        private NameValueCollection _bodyParameters;
        private Type _type;
        private HttpMethod _method;
        private string _response;

        public RequestBuilder()
        {            
            _type = typeof(string);

            _parameters = HttpUtility.ParseQueryString(string.Empty);

            _requestHeaders = new NameValueCollection();

            _bodyParameters = new NameValueCollection();

            _method = HttpMethod.GET;
        }

        public IFluentOperation Create(string url)
        {
            throw new NotImplementedException();
        }

        public IFluentOperation AddParam(string param, string value)
        {
            throw new NotImplementedException();
        }

        public IFluentOperation AddHeader(string header, string value)
        {
            throw new NotImplementedException();
        }

        public IFluentOperation AddBodyParam(string param, string value)
        {
            throw new NotImplementedException();
        }

        public IFluentProcess Get()
        {
            throw new NotImplementedException();
        }

        public IFluentProcess Post()
        {
            throw new NotImplementedException();
        }

        public Task<IFluentProcess> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IFluentProcess> PostAsync()
        {
            throw new NotImplementedException();
        }

        public string GetQueryString(bool printPort = false)
        {
            throw new NotImplementedException();
        }

        public IFluentTransform Extract(string path)
        {
            throw new NotImplementedException();
        }

        public T Fill<T>()
        {
            throw new NotImplementedException();
        }
        //public static RequestBuilder Create(string endpoint)
        //{
        //    return new RequestBuilder() { _endpoint = new Uri(endpoint) };
        //}
        //public RequestBuilder AddParam(string param, string value)
        //{
        //    _parameters.Add(param, value);
        //    return this;
        //}
        //public RequestBuilder AddParam<T>(T value) where T : class
        //{
        //    _parameters.Add(value.ToNameCollection());
        //    return this;
        //}
        //public RequestBuilder AddParam<T>(List<T> values) where T : class
        //{
        //    values.ForEach(value => _parameters.Add(value.ToNameCollection()));
        //    return this;
        //}
        //public RequestBuilder AddHeader(string header, string value)
        //{
        //    _requestHeaders.Add(header, value);
        //    return this;
        //}
        //public RequestBuilder AddBodyParam(string param, string value)
        //{
        //    _bodyParameters.Add(param, value);
        //    return this;
        //}
        //public string GetQueryString(bool printPort = false)
        //{
        //    var uriBuilder = new UriBuilder(_endpoint) { Query = _parameters.ToString() };
        //    if (!printPort) uriBuilder.Port = -1;
        //    return uriBuilder.ToString();
        //}
        //public RequestBuilder Method(HttpMethod method)
        //{
        //    _method = method;
        //    return this;
        //}
        //public T Fill<T>()
        //{
        //    return JsonConvert.DeserializeObject<T>(_response);
        //}
        //public RequestBuilder Extract(string path)
        //{
        //    JToken jsonResponse = JToken.Parse(_response);
        //    _response = jsonResponse.SelectToken(path).ToString();
        //    return this;
        //}
        //public RequestBuilder Get()
        //{
        //    _response = Http.Get(GetQueryString(), _requestHeaders);
        //    return this;
        //}
        //public RequestBuilder Post()
        //{
        //    _response = Http.Post(GetQueryString(), _requestHeaders, _bodyParameters);
        //    return this;
        //}
        //public Task<RequestBuilder> GetAsync()
        //{
        //    return Task.Run(() =>
        //    {
        //        return Get();
        //    });
        //}
        //public Task<RequestBuilder> PostAsync()
        //{
        //    return Task.Run(() =>
        //    {
        //        return Get();
        //    });
        //}





    }
}
