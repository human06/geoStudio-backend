using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace geoStudio.Infrastructure.Caching;

/// <summary>Extension methods for registering Redis caching services.</summary>
public static class CacheExtensions
{
    /// <summary>
    /// Registers StackExchange.Redis as the <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/>
    /// provider and wires up <see cref="ICacheService"/> via <see cref="RedisCacheService"/>.
    /// </summary>
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("RedisConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'RedisConnection' is missing from configuration.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "geoStudio:";
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
