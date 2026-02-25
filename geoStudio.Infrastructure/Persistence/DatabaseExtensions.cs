using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace geoStudio.Infrastructure.Persistence;

/// <summary>Extension methods for registering PostgreSQL / EF Core services.</summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Registers <see cref="GeoStudioDbContext"/> with Npgsql, retry-on-failure, and
    /// explicit Postgres server version targeting.
    /// </summary>
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is missing from configuration.");

        services.AddDbContext<GeoStudioDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions
                    .EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null)
                    .SetPostgresVersion(15, 0)
                    .MigrationsAssembly(typeof(GeoStudioDbContext).Assembly.FullName)
            ));

        return services;
    }
}
