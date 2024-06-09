using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class MemoryCacheExtensions
    {       
            public static DateTime GetCreationTime(this IMemoryCache cache, string key)
            {
                if (!cache.TryGetValue(key, out object _))
                {
                    throw new ArgumentException("Key does not exist in the cache.");
                }

                return (DateTime)cache.Get(key + ":created");
            }
        
    }
}
