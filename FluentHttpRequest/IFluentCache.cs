using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.CacheExtension
{
    public interface IFluentCacheAction
    {
        void Add(object value, object key, string region, CacheItemPriority cachePriority = CacheItemPriority.NotRemovable);
        void AddRange<T>(IEnumerable<T> collection, string key, string region, CacheItemPriority cachePriority = CacheItemPriority.NotRemovable);
        T Get<T>(object key, string region);
        void Remove(object key, string region);
        IEnumerable<T> GetAll<T>(string region);
        bool HasItems();
        void RemoveAll(string region);
        void Update<T>(object key, string region, T updateObject);
    }    
}
