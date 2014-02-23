using System;
using System.Collections.Generic;

namespace PreScripds.Infrastructure
{
    public class NullCacheProvider : ICacheProvider
    {
        public void Set(string key, object value, TimeSpan cacheDuration)
        {

        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool Remove(string key)
        {
            return false;
        }

        public void FlushAll()
        {

        }

        public IEnumerable<string> GetAllCacheItems()
        {
            return default(IEnumerable<string>);
        }
    }
}