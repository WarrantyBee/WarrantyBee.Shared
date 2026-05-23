using Microsoft.Extensions.DependencyInjection;
using WarrantyBee.Shared.Security.Abstractions;
using WarrantyBee.Shared.Security.Services;

namespace WarrantyBee.Shared.Security;

/// <summary>
/// Provides extension methods for registering security-related services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers unified security protocols and authentication layers.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddWarrantyBeeSecurity(this IServiceCollection services)
    {
        services.AddScoped<IApiKeyService, ApiKeyService>();
        return services;
    }
}
