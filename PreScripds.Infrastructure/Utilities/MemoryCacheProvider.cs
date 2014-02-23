using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;

namespace PreScripds.Infrastructure
{
    public class MemoryCacheProvider : ICacheProvider
    {
        static MemoryCache cacheInstance = CreateMemoryCache();

        private static MemoryCache CreateMemoryCache()
        {
            return new MemoryCache(AppDomain.CurrentDomain.FriendlyName);
        }

        public void Set(string key, object value, TimeSpan cacheDuration)
        {
            cacheInstance.Set(key, value, DateTimeOffset.Now.Add(cacheDuration));
        }

        public T Get<T>(string key)
        {
            object val = cacheInstance.Get(key);
            return val == null ? default(T) : (T)val;
        }

        public bool Remove(string key)
        {
            return cacheInstance.Remove(key) != null;
        }

        public void FlushAll()
        {
            cacheInstance.Dispose();
            cacheInstance = CreateMemoryCache();
        }

        public IEnumerable<string> GetAllCacheItems()
        {
            return cacheInstance.Select(cacheItem => cacheItem.Key);
        }
    }
}