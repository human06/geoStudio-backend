using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace geoStudio.BackgroundJobs;

/// <summary>Extension methods for registering Hangfire background-job infrastructure.</summary>
public static class HangfireExtensions
{
    /// <summary>
    /// Configures Hangfire to use PostgreSQL storage (shared connection with EF Core),
    /// registers the Hangfire server with named queues, and wires up the background jobs
    /// defined in this assembly.
    /// </summary>
    public static IServiceCollection AddHangfireServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is missing from configuration.");

        services.AddHangfire(config =>
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(pg =>
                    pg.UseNpgsqlConnection(connectionString)));

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = Environment.ProcessorCount * 2;
            options.ServerName   = "geoStudio-jobs";
            options.Queues       = ["critical", "audits", "reports", "default"];
        });

        return services;
    }
}
