using System;
using System.Collections.Specialized;
using FluentHttpRequest.Helpers;
using System.Web;

namespace FluentHttpRequest
{
    public class RequestBuilder
    {
        public Uri Endpoint { get; set; }
        public NameValueCollection Parameters { set; get; }

        public NameValueCollection RequestHeaders { set; get; }

        public Type FillType { set; get; }

        public HttpMethod Method { get; set; }

        public string StringResponse { get; set; }


        private RequestBuilder()
        {
            FillType = typeof(string);

            Parameters = HttpUtility.ParseQueryString(string.Empty);

            RequestHeaders = new  NameValueCollection();

            Method = HttpMethod.GET;
        }

        public static RequestBuilder Create(string endpoint)
        {
            return new RequestBuilder() { Endpoint = new Uri(endpoint) };
        }

        public RequestBuilder Execute()
        {
            switch (Method)
            {
                case HttpMethod.GET:
                    StringResponse = Http.Get(Endpoint, Parameters, RequestHeaders);
                    break;
                case HttpMethod.POST:
                    StringResponse = Http.Post(Endpoint, Parameters, RequestHeaders);
                    break;
                default:
                    StringResponse = Http.Get(Endpoint, Parameters, RequestHeaders);
                    break;
            }

            return this;
        }
    }
}
