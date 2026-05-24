using WarrantyBee.Shared.Core.Enums;

namespace WarrantyBee.Shared.Security.Abstractions;

/// <summary>
/// Represents the security context resolved for an API Client.
/// </summary>
public class ApiClientContext
{
    public long ClientId { get; set; }
    public string AppId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public SecurityRole Role { get; set; }
    public IEnumerable<SecurityPermission> Permissions { get; set; } = [];
}

/// <summary>
/// Defines a service for validating stateful API keys and restricted endpoints.
/// </summary>
public interface IApiKeyService
{
    /// <summary>
    /// Validates an AppId and AppSecret against the cache or database.
    /// </summary>
    /// <param name="appId">The unique Application ID.</param>
    /// <param name="appSecret">The plain-text Application Secret.</param>
    /// <param name="requestedPath">The current request path to validate against allowed endpoints.</param>
    /// <returns>The security context if valid; otherwise, null.</returns>
    Task<ApiClientContext?> ValidateAsync(string appId, string appSecret, string requestedPath);

    /// <summary>
    /// Validates a standalone API Key against the cache or database.
    /// </summary>
    /// <param name="apiKey">The plain-text API Key string.</param>
    /// <param name="requestedPath">The current request path to validate against allowed endpoints.</param>
    /// <returns>The security context if valid; otherwise, null.</returns>
    Task<ApiClientContext?> ValidateKeyAsync(string apiKey, string requestedPath);
}
