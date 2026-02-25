using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace geoStudio.Infrastructure.Persistence;

/// <summary>
/// Design-time factory used by the EF Core CLI (<c>dotnet ef migrations add</c>) when
/// running against the Infrastructure project without starting the full API host.
/// It reads the connection string from environment variables or falls back to a
/// local development placeholder so the CLI can scaffold migrations offline.
/// </summary>
public sealed class GeoStudioDbContextFactory : IDesignTimeDbContextFactory<GeoStudioDbContext>
{
    public GeoStudioDbContext CreateDbContext(string[] args)
    {
        // Prefer an explicit env var so CI/CD can override without touching source files.
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? BuildFromEnvironmentParts()
            ?? "Host=localhost;Port=5432;Database=geostudio_dev;Username=postgres;Password=postgres;";

        var optionsBuilder = new DbContextOptionsBuilder<GeoStudioDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsql => npgsql
                .MigrationsAssembly(typeof(GeoStudioDbContext).Assembly.FullName)
                .SetPostgresVersion(15, 0));

        return new GeoStudioDbContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Assembles a connection string from the granular POSTGRES_* env vars
    /// set in <c>.env.example</c> / <c>.env</c>.
    /// </summary>
    private static string? BuildFromEnvironmentParts()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        var db   = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(db) ||
            string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            return null;

        return $"Host={host};Port={port};Database={db};Username={user};Password={pass};";
    }
}
