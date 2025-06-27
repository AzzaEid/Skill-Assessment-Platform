using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SkillAssessmentPlatform.Infrastructure.Services
{

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CacheService> _logger;
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public CacheService(IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        private static string Serialize<T>(T value) => JsonSerializer.Serialize(value);
        private static T? Deserialize<T>(string? value) where T : class
                        => string.IsNullOrEmpty(value) ? null : JsonSerializer.Deserialize<T>(value);


        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (_memoryCache.TryGetValue(key, out T? memValue))
                {
                    _logger.LogInformation("in memory");
                    return memValue;
                }

                var redisValue = await _distributedCache.GetStringAsync(key);
                var value = Deserialize<T>(redisValue);
                _logger.LogInformation("in redis");

                if (value != null)
                    _memoryCache.Set(key, value, TimeSpan.FromMinutes(5));

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cache key '{Key}'", key);
                return null;
            }
        }

        public async Task CreateAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
            {
                var serialized = Serialize(value);

                var distOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
                };

                await _distributedCache.SetStringAsync(key, serialized, distOptions);
                _memoryCache.Set(key, value, expiration ?? TimeSpan.FromMinutes(30));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache key '{Key}'", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
                _memoryCache.Remove(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache key '{Key}'", key);
            }
        }

        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? expiration = null) where T : class
        {
            var cached = await GetAsync<T>(key);
            if (cached != null)
                return cached;

            var value = await getItem();
            if (value != null)
                await CreateAsync(key, value, expiration);

            return value;
        }
    }

}
