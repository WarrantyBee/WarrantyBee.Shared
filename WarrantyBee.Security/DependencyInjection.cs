using Microsoft.Extensions.DependencyInjection;
using WarrantyBee.Shared.Security.Abstractions;
using WarrantyBee.Shared.Security.Services;

namespace WarrantyBee.Shared.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddWarrantyBeeSecurity(this IServiceCollection services)
    {
        services.AddScoped<IApiKeyService, ApiKeyService>();
        return services;
    }
}
