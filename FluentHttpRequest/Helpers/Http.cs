using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace FluentHttpRequest.Helpers
{
    public class Http
    {
        public static string Get(Uri endopoint, NameValueCollection parameters, NameValueCollection headers)
        {
            return Request(HttpMethod.GET, endopoint, parameters, headers);
        }

        public static string Post(Uri endopoint, NameValueCollection parameters, NameValueCollection headers)
        {
            return Request(HttpMethod.POST, endopoint, parameters, headers);
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

        private static string Request(HttpMethod method, Uri endopoint, NameValueCollection parameters = null, NameValueCollection headers = null)
        {
            string strResponse = string.Empty;

            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(endopoint);

            request.Method = method.ToString();

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                if (headers != null) request.Headers.Add(headers);

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
