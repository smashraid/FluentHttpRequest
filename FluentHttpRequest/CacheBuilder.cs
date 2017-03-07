using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.CacheExtension
{
    public class Cache : IFluentCacheAction
    {
        private static ObjectCache memoryCache = MemoryCache.Default;
        
        public static Cache Storage
        {
            get { return new Cache(); }
        }

        private Cache()
        {

        }
        private string Item(string key, string region)
        {
            return $"{key}/{region}";
        }
        public void AddRange<T>(IEnumerable<T> collection, string key, string region, CacheItemPriority cachePriority = CacheItemPriority.NotRemovable)
        {
            foreach (T item in collection)
            {
                object value = item.GetType().GetProperty(key).GetValue(item);
                Add(item, value.ToString(), region, cachePriority);
            }
        }
        public void Add(object value, string key, string region, CacheItemPriority cachePriority = CacheItemPriority.NotRemovable)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = cachePriority;
            policy.AbsoluteExpiration = DateTimeOffset.MaxValue;
            string k = Item(key, region);
            if (!memoryCache.Contains(k))
            {
                memoryCache.Set(k, value, policy);
            }
        }
        public T Get<T>(string key, string region)
        {
            return (T)memoryCache.Get(Item(key, region));
        }
        public void Remove(string key, string region)
        {
            string k = Item(key, region);
            if (memoryCache.Contains(k))
            {
                memoryCache.Remove(k);
            }
        }
        public IEnumerable<T> GetAll<T>(string region) 
        {
            IEnumerable<T> values = memoryCache
                .Where(x => x.Key.Contains(region))
                .Select(x=>  (T)x.Value)
                .ToList();
            return values;
        }
        public bool HasItems()
        {
            return memoryCache.GetCount() > 0;
        }
        public void RemoveAll(string region)
        {
            memoryCache.Remove(region);
        }
        public void RefreshCache<T>(string key, string region, T updateObject)
        {
            IDictionary<string, object> removeObject = (IDictionary<string, object>)Get<T>(key, region);
            foreach (KeyValuePair<string, object> keyValuePair in (IDictionary<string, object>)updateObject)
            {
                if (removeObject.ContainsKey(keyValuePair.Key))
                {
                    removeObject[keyValuePair.Key] = ((IDictionary<string, object>)updateObject)[keyValuePair.Key];
                }
            }
        }
    }
}