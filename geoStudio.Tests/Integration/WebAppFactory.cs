using geoStudio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace geoStudio.Tests.Integration;

/// <summary>Custom WebApplicationFactory for integration testing with an in-memory database.</summary>
public sealed class WebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace real DB with in-memory for tests
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<GeoStudioDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<GeoStudioDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestDb"));
        });

        builder.UseEnvironment("Testing");
    }
}
