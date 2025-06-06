namespace ApiGateway.CacheRedis.Cache
{
    public interface ICacheRedis
    {
        Task SetAsync(string cacheKey, object response, TimeSpan timeLive);
        Task<string> GetAsync(string cacheKey);
        Task<T> GetAsync<T>(string cacheKey);
        Task ClearAsync(string cacheKey);
        void Set(string cacheKey, object response, TimeSpan timeLive);
        string Get(string cacheKey);
        T Get<T>(string cacheKey);
        void Clear(string cacheKey);
        Task IncreaseIntAsync(string cacheKey, TimeSpan timeLive);
    }
}
