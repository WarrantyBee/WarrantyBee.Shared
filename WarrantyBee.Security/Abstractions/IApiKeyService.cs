namespace WarrantyBee.Shared.Security.Abstractions;

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
    /// <returns>True if the credentials and path are valid; otherwise, false.</returns>
    Task<bool> ValidateAsync(string appId, string appSecret, string requestedPath);

    /// <summary>
    /// Validates a standalone API Key against the cache or database.
    /// </summary>
    /// <param name="apiKey">The plain-text API Key string.</param>
    /// <param name="requestedPath">The current request path to validate against allowed endpoints.</param>
    /// <returns>True if the API Key and path are valid; otherwise, false.</returns>
    Task<bool> ValidateKeyAsync(string apiKey, string requestedPath);
}
