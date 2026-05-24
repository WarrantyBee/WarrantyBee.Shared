using Microsoft.Extensions.DependencyInjection;
using WarrantyBee.Shared.Infrastructure.Abstractions;
using WarrantyBee.Shared.Infrastructure.Filters;
using WarrantyBee.Shared.Infrastructure.Services;
using WarrantyBee.Shared.Infrastructure.Persistence;

namespace WarrantyBee.Shared.Infrastructure;

/// <summary>
/// Provides extension methods for registering shared infrastructure services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers high-scale infrastructure blueprints, including telemetry, caching, and background processing.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddWarrantyBeeInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddScoped<ITelemetryService, TelemetryService>();
        services.AddScoped<ICacheService, UpstashCacheService>();
        services.AddScoped<IEventPublisher, EventPublisher>();
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<IOcrService, SmartOcrService>();
        
        // Register filters
        services.AddScoped<TelemetryActionFilter>();
        
        return services;
    }
}
