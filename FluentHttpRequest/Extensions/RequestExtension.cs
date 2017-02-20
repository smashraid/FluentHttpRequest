using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FluentHttpRequest
{
    public static class RequestExtension
    {
        public static RequestBuilder AddParam(this RequestBuilder builder, string param, string value)
        {
            builder.Parameters.Add(param, value);

            return builder;
        }

        public static RequestBuilder AddParam<T>(this RequestBuilder builder, T value) where T : class  
        {
            builder.Parameters.Add(value.ToNameCollection());

            return builder;
        }

        public static RequestBuilder AddParam<T>(this RequestBuilder builder, List<T> values) where T : class
        {
            values.ForEach(value => builder.Parameters.Add(value.ToNameCollection()));

            return builder;
        }

        #region "Request"
        public static RequestBuilder AddHeader(this RequestBuilder builder, string header , string value)
        {
            builder.RequestHeaders.Add(header, value);

            return builder;
        }

       
        public static string GetQueryString(this RequestBuilder builder, bool printPort = false)
        {
            var uriBuilder = new UriBuilder(builder.Endpoint) { Query = builder.Parameters.ToString() };

            if (!printPort) uriBuilder.Port = -1;

            return uriBuilder.ToString();
        }

        public static RequestBuilder Method(this RequestBuilder builder, HttpMethod method)
        {
            builder.Method = method;

            return builder;
        }
        #endregion

        public static object Fill<T>(this RequestBuilder builder)
        {
            return JsonConvert.DeserializeObject<T>(builder.StringResponse);
        }

        public static RequestBuilder Extract(this RequestBuilder builder, string path)
        {
            JToken jsonResponse = JToken.Parse(builder.StringResponse);

            JToken token = null;

            if (jsonResponse is JArray)
            {
                JArray jArray = JArray.Parse(builder.StringResponse);

                token = jArray.SelectToken(path);
            }
            else if (jsonResponse is JObject)
            {
                JObject jObject = JObject.Parse(builder.StringResponse);

                token = jObject.SelectToken(path);
            }

            builder.StringResponse = token.ToString();

            return builder;
        }

        public static RequestBuilder Extract2(this RequestBuilder builder, string path)
        {
            JToken jsonResponse = JToken.Parse(builder.StringResponse);

            builder.StringResponse = jsonResponse.SelectToken(path).ToString();

            return builder;
        }
    }
}
