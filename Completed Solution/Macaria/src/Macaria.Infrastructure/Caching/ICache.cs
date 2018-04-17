using System;
using System.Threading.Tasks;

namespace Macaria.Infrastructure.Caching
{
    public interface ICache
    {
        T Get<T>(string key);

        object Get(string key);

        void Add(object objectToCache, string key);

        void Add<T>(object objectToCache, string key);

        void Add<T>(object objectToCache, string key, double cacheDuration);

        void Remove(string key);

        void ClearAll();

        bool Exists(string key);

        TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key, double cacheDuration);

        Task<TResponse> FromCacheOrServiceAsync<TResponse>(Func<Task<TResponse>> action, string key, double cacheDuration);

        TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key);

        Task<TResponse> FromCacheOrServiceAsync<TResponse>(Func<Task<TResponse>> action, string key);
    }
}
