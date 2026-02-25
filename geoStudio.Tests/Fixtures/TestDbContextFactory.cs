using geoStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace geoStudio.Tests.Fixtures;

/// <summary>Creates an in-memory EF Core context for unit and integration tests.</summary>
public static class TestDbContextFactory
{
    public static GeoStudioDbContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<GeoStudioDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new GeoStudioDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
