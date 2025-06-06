using Microsoft.Extensions.Caching.Distributed;
using ServiceStack;

namespace ApiGateway.CacheRedis.Cache
{
    public class CacheRedis : ICacheRedis
    {
        private readonly IDistributedCache _distributedCache;
        public CacheRedis(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<string> GetAsync(string cacheKey)
        {
            var json = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(json) ? null : json;
        }
        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var json = await _distributedCache.GetStringAsync(cacheKey);
            if (json != null && !string.IsNullOrEmpty(json))
            {
                try
                {
                    return json.FromJson<T>();
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }
        public async Task SetAsync(string cacheKey, object response, TimeSpan timeLive)
        {
            if (response == null)
            {
                return;
            }
            var json = response.ToJson();
            await _distributedCache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeLive
            });
        }

        public async Task IncreaseIntAsync(string cacheKey, TimeSpan timeLive)
        {
            var json = await _distributedCache.GetStringAsync(cacheKey);
            if (json == null || string.IsNullOrEmpty(json))
            {
                await SetAsync(cacheKey, 1, timeLive);
                return;
            }
            var totalCount = int.Parse(json);
            await SetAsync(cacheKey, totalCount + 1, timeLive);
        }
        public async Task ClearAsync(string cacheKey)
        {
            await _distributedCache.RemoveAsync(cacheKey);
        }
        public string Get(string cacheKey)
        {
            var json = _distributedCache.GetString(cacheKey);
            return string.IsNullOrEmpty(json) ? null : json;
        }
        public T Get<T>(string cacheKey)
        {
            var json = _distributedCache.GetString(cacheKey);
            if (json != null && !string.IsNullOrEmpty(json))
            {
                try
                {
                    return json.FromJson<T>();
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }
        public void Set(string cacheKey, object response, TimeSpan timeLive)
        {
            if (response == null)
            {
                return;
            }
            var json = response.ToJson();
            _distributedCache.SetString(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeLive
            });
        }
        public void Clear(string cacheKey)
        {
            _distributedCache.Remove(cacheKey);
        }
    }
}

