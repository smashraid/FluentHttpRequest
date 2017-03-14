using System.Linq;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;

namespace FluentHttpRequest
{
    public static class ConvertExtension
    {
        public static NameValueCollection ToNameCollection<T>(this T objectClass) where T : class
        {
            NameValueCollection parameters = new  NameValueCollection();

            objectClass.GetType().GetProperties().ToList().ForEach(prop => parameters.Add(prop.Name, prop.GetValue(objectClass).ToString()));

            return parameters;
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
    }
}
