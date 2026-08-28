namespace Exp.Shared.Caching;

public sealed class CacheSettings
{
    public string Provider { get; set; } = "Redis";
    public CachePolicySettings DefaultPolicy { get; set; } = new();
    public Dictionary<string, RedisCacheProviderSettings> Providers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class CachePolicySettings
{
    public TimeSpan? Expiration { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
}

public sealed class RedisCacheProviderSettings
{
    public string? ConnectionString { get; set; }
    public string InstanceName { get; set; } = "ExpWeatherCache";
}
