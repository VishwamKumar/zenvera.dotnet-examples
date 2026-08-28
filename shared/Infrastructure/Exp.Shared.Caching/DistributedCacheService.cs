namespace Exp.Shared.Caching;

internal sealed class DistributedCacheService(IDistributedCache cache, CacheSettings settings) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var bytes = await cache.GetAsync(key, cancellationToken);
        if (bytes is null || bytes.Length == 0)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(bytes, JsonOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? slidingExpiration = null,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
        var options = new DistributedCacheEntryOptions();

        var sliding = slidingExpiration ?? settings.DefaultPolicy.SlidingExpiration;
        if (sliding is { } slidingValue)
        {
            options.SetSlidingExpiration(slidingValue);
        }

        var absolute = absoluteExpiration ?? settings.DefaultPolicy.Expiration;
        if (absolute is { } absoluteValue)
        {
            options.SetAbsoluteExpiration(absoluteValue);
        }

        await cache.SetAsync(key, bytes, options, cancellationToken);
    }
}
