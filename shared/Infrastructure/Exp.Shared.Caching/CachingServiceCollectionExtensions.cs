namespace Exp.Shared.Caching;

public static class CachingServiceCollectionExtensions
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var flags = configuration.GetFeatureFlags();
        var settings = configuration.BindResolved<CacheSettings>("CacheSettings");
        services.AddSingleton(settings);

        if (!flags.UseCaching)
        {
            services.AddSingleton<ICacheService, NullCacheService>();
            return services;
        }

        if (string.Equals(settings.Provider, "InMemory", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var redis = GetRedisSettings(settings);
            var connectionString = redis.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = BuildLegacyRedisConnectionString(configuration);
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Redis caching is enabled but no connection string was found. Set CacheSettings:Providers:Redis:ConnectionString or EXP_REDIS_CONNECTION_STRING (or RedisCacheSettings).");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = redis.InstanceName;
            });
        }

        services.AddSingleton<ICacheService, DistributedCacheService>();
        return services;
    }

    private static RedisCacheProviderSettings GetRedisSettings(CacheSettings settings)
        => settings.Providers.TryGetValue("Redis", out var redis)
            ? redis
            : new RedisCacheProviderSettings();

    private static string? BuildLegacyRedisConnectionString(IConfiguration configuration)
    {
        var legacy = configuration.GetSection("RedisCacheSettings");
        if (!legacy.Exists())
        {
            return null;
        }

        var host = EnvironmentPlaceholder.Resolve(legacy["Host"]) ?? "localhost";
        var port = legacy["Port"] ?? "6379";
        var database = legacy["DB"] ?? "0";
        var password = EnvironmentPlaceholder.Resolve(legacy["Password"]);
        return string.IsNullOrWhiteSpace(password)
            ? $"{host}:{port},defaultDatabase={database}"
            : $"{host}:{port},password={password},ssl=False,defaultDatabase={database}";
    }
}
