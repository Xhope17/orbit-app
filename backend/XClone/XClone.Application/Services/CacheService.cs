using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using XClone.Application.Interfaces.Services;

namespace XClone.Application.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        public T Create<T>(string key, TimeSpan expiration, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            var json = JsonSerializer.Serialize(value);
            distributedCache.SetString(key, json, options);
            return value;
        }

        public T? Get<T>(string key)
        {
            var json = distributedCache.GetString(key);
            if (json is null) return default;
            return JsonSerializer.Deserialize<T>(json);
        }

        public bool Delete(string key)
        {
            try
            {
                distributedCache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
