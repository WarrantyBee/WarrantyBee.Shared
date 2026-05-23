namespace WarrantyBee.Shared.Infrastructure.Abstractions;

/// <summary>
/// Defines a service for high-performance caching.
/// </summary>
public interface ICacheService
{
    Task SetAsync(string key, string value, int? expirySeconds = null);
    Task<string?> GetAsync(string key);
    Task DeleteAsync(string key);
}
