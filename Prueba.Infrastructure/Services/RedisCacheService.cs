using Newtonsoft.Json;
using Prueba.Core.Services;
using StackExchange.Redis;

namespace Prueba.Infrastructure.Services;

public class RedisCacheService: ICacheService
{
    private readonly IDatabase _cacheDatabase;
    private readonly string _instanceName;

    public RedisCacheService(IConnectionMultiplexer redisConnection, string instanceName)
    {
        _cacheDatabase = redisConnection.GetDatabase();
        _instanceName = instanceName;
    }
    
    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _cacheDatabase.StringGetAsync(_instanceName + key);
        if (value.IsNullOrEmpty)
            return default;

        return JsonConvert.DeserializeObject<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var jsonValue = JsonConvert.SerializeObject(value);
        await _cacheDatabase.StringSetAsync(_instanceName + key, jsonValue, expiry);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await _cacheDatabase.KeyDeleteAsync(_instanceName + key);
    }
}