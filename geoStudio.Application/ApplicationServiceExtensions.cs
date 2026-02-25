using FluentValidation;
using geoStudio.Application.Interfaces;
using geoStudio.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace geoStudio.Application;

/// <summary>Extension methods for registering Application layer services.</summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBusinessService, BusinessService>();

        // Register all FluentValidation validators from this assembly
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceExtensions).Assembly);

        return services;
    }
}
