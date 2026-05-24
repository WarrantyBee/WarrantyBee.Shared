namespace WarrantyBee.Shared.Security.Abstractions;

/// <summary>
/// Defines a repository for validating stateful API keys and endpoint permissions.
/// </summary>
public interface IApiKeyRepository
{
    /// <summary>
    /// Validates an AppId and secret hash against the database, ensuring the requested endpoint is allowed.
    /// </summary>
    /// <param name="appId">The unique Application ID.</param>
    /// <param name="secretHash">The hashed Application Secret.</param>
    /// <param name="requestedPath">The current request path to validate.</param>
    /// <returns>True if valid and active; otherwise, false.</returns>
    Task<bool> ValidateAsync(string appId, string secretHash, string requestedPath);

    /// <summary>
    /// Validates a standalone API key hash against the database, ensuring the requested endpoint is allowed.
    /// </summary>
    /// <param name="keyHash">The hashed API Key.</param>
    /// <param name="requestedPath">The current request path to validate.</param>
    /// <returns>True if valid and active; otherwise, false.</returns>
    Task<bool> ValidateKeyAsync(string keyHash, string requestedPath);
}
