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
        //T Add<T>(T t);
        void Add(object value, string key, string region, CacheItemPriority cachePriority = CacheItemPriority.NotRemovable);
        void AddRange<T>(IEnumerable<T> collection, string key, string region, CacheItemPriority cachePriority = CacheItemPriority.NotRemovable);
        T Get<T>(string key, string region);
        void Remove(string key, string region);
        IEnumerable<T> GetAll<T>(string region);
        bool HasItems();
        void RemoveAll(string region);
        void RefreshCache<T>(string key, string region, T updateObject);
    }

    public interface IFluentCacheExtension
    {
        T AddCache<T>(T t);
    }
}
