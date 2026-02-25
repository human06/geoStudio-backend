using geoStudio.Application.Interfaces;
using geoStudio.Infrastructure.Caching;
using geoStudio.Infrastructure.Identity;
using geoStudio.Infrastructure.Persistence;
using geoStudio.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace geoStudio.Infrastructure;

/// <summary>Extension methods for registering Infrastructure layer services.</summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // PostgreSQL / EF Core
        services.AddDbContext<GeoStudioDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(GeoStudioDbContext).Assembly.FullName)));

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "geoStudio:";
        });

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        // Identity
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

        // Cache
        services.AddScoped<IRedisCacheService, RedisCacheService>();

        return services;
    }
}
