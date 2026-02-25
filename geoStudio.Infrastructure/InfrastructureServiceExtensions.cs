using geoStudio.Application.Interfaces;
using geoStudio.Infrastructure.Caching;
using geoStudio.Infrastructure.Identity;
using geoStudio.Infrastructure.Persistence;
using geoStudio.Infrastructure.Persistence.Repositories;
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
        services.AddDatabase(configuration);
        services.AddCaching(configuration);

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        // Identity
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

        return services;
    }
}
