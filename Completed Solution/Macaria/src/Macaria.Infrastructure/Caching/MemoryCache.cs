using Macaria.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Macaria.Infrastructure.Caching
{
    public class MemoryCache : Cache
    {
        private static IMemoryCache _cache;
        private string _tenant;
        public MemoryCache(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = memoryCache;
            _tenant = httpContextAccessor.HttpContext.Request.GetHeaderValue("Tenant");            
        }
        
        public override T Get<T>(string key) => (T)Get(key);

        public override object Get(string key)
        {
            dynamic value;
            _cache.TryGetValue($"{key}-{_tenant}", out value);
            return value;
        }

        public override void Add(object objectToCache, string key)
        {
            if (objectToCache == null)
            {
                _cache.Remove($"{key}-{_tenant}");
            }
            else
            {
                _cache.Set($"{key}-{_tenant}", objectToCache);
            }
        }

        public override void Add<T>(object objectToCache, string key) => Add(objectToCache, key);

        public override void Add<T>(object objectToCache, string key, double cacheDuration)
        {
            DateTime cacheEntry;

            if (objectToCache == null)
            {
                _cache.Remove($"{key}-{_tenant}");
            }
            else
            {
                cacheEntry = DateTime.Now;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(cacheDuration));

                _cache.Set($"{key}-{_tenant}", objectToCache, cacheEntryOptions);
            }
        }

        public override void Remove(string key) => _cache.Remove(key);

        public override void ClearAll()
        {
            throw new Exception();
        }

        public override bool Exists(string key) => throw new Exception();
    }
}
