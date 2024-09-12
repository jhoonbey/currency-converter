using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CurrencyConverter.Services;

public class CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger) : ICacheService
{
    public async Task<T?> GetDataAsync<T>(string key)
    {
        var value = await distributedCache.GetStringAsync(key);
        return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value) : default;
    }

    public async Task SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = expirationTime
        };
        await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }
}