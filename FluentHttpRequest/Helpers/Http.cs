using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace FluentHttpRequest.Helpers
{
    public class Http
    {
        public static string Get(string endopoint, NameValueCollection headers)
        {
            return Request(HttpMethod.GET, endopoint, headers);
        }

        public static string Post(string endopoint, NameValueCollection headers, NameValueCollection bodyParameters)
        {
            return Request(HttpMethod.POST, endopoint, headers, bodyParameters);
        }

        public static byte[] Download(string url)
        {
            byte[] download;

            using (WebClient client = new WebClient())
            {
                download = client.DownloadData(url);
            }

            return download;
        }

        private static string Request(
            HttpMethod method, 
            string endopoint, 
            NameValueCollection headers = null)
        {
            string strResponse = string.Empty;    
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(endopoint);
            request.Method = method.ToString();

            if (headers != null) request.Headers.Add(headers);

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {               
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                strResponse = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }

            return strResponse;
        }

        private static string Request(
         HttpMethod method,
         string endopoint,
         NameValueCollection headers = null,
         NameValueCollection bodyParameters = null)
        {
            string strResponse = string.Empty;
            string postData = string.Empty;

            foreach (string key in bodyParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(bodyParameters[key]) + "&";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endopoint);

            request.Method = method.ToString();

            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            if (headers != null) request.Headers.Add(headers);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                strResponse = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }

            return strResponse;
        }
    }
}
