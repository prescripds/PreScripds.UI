using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreScripds.Infrastructure
{
    public static class DictionaryExtensions
    {
        public static TValue SafeGet<TKey,TValue>(this IDictionary<TKey,TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue val = defaultValue;
            if(dict != null && key != null)
            {
                dict.TryGetValue(key, out val);
            }
            return val;
        }
    }
}
