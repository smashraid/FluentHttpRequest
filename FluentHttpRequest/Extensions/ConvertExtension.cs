using System.Linq;
using System.Collections.Specialized;

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
    }
}
