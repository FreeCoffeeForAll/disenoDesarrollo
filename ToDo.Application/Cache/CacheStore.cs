using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Cache;

namespace ToDo.Application.Cache
{
    public class CacheStore : ICacheStore
    {
        private readonly IMemoryCache _cacheManager;
        private readonly IDictionary<string, TimeSpan> _cacheKeys;

        public CacheStore(IMemoryCache cacheManager,
            IOptions<Dictionary<string, TimeSpan>> cacheKeys)
        {
            _cacheManager = cacheManager;
            _cacheKeys = cacheKeys.Value;
        }

        public void Add<T>(T item, ICacheKey<T> key)
            where T : class
        {
            Type type = typeof(T);
            string typeName = type.Name;

            _cacheKeys.TryGetValue(typeName, out TimeSpan expires);
            if (expires != default)
            {
                _cacheManager.Set(key.Key, item, expires);
            }
            else
            {
                _cacheManager.Set(key.Key, item);
            }
        }

        public T Get<T>(ICacheKey<T> key)
            where T : class
        {
            return (T)_cacheManager.Get(key.Key);
        }
    }
}
