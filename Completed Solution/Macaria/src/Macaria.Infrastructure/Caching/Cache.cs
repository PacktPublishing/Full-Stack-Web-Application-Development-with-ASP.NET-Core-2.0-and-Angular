using System;
using System.Threading.Tasks;

namespace Macaria.Infrastructure.Caching
{
    public abstract class Cache : ICache
    {
        public abstract void Add(object objectToCache, string key);
        public abstract void Add<T>(object objectToCache, string key);
        public abstract void Add<T>(object objectToCache, string key, double cacheDuration);
        public abstract void ClearAll();
        public abstract bool Exists(string key);
        public abstract object Get(string key);
        public abstract T Get<T>(string key);
        public abstract void Remove(string key);

        public virtual TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key, double cacheDuration)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = action();
                Add<TResponse>(cached, key, cacheDuration);
            }
            return (TResponse)cached;
        }

        public async Task<TResponse> FromCacheOrServiceAsync<TResponse>(Func<Task<TResponse>> action, string key, double cacheDuration)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = await action();
                Add<TResponse>(cached, key, cacheDuration);
            }
            return (TResponse)cached;
        }

        public virtual TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = action();
                Add(cached, key);
            }
            return (TResponse)cached;
        }

        public async Task<TResponse> FromCacheOrServiceAsync<TResponse>(Func<Task<TResponse>> action, string key)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = await action();
                Add(cached, key);
            }
            return (TResponse)cached;
        }
    }
}
