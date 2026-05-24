namespace WarrantyBee.Shared.Security.Abstractions;

/// <summary>
/// Defines a repository for validating stateful API keys and resolving security context.
/// </summary>
public interface IApiKeyRepository
{
    /// <summary>
    /// Validates an AppId and secret hash against the database and returns the client context.
    /// </summary>
    /// <param name="appId">The unique Application ID.</param>
    /// <param name="secretHash">The hashed Application Secret.</param>
    /// <param name="requestedPath">The current request path to validate.</param>
    /// <returns>The security context if valid and authorized; otherwise, null.</returns>
    Task<ApiClientContext?> ResolveAsync(string appId, string secretHash, string requestedPath);

    /// <summary>
    /// Validates a standalone API key hash against the database and returns the client context.
    /// </summary>
    /// <param name="keyHash">The hashed API Key.</param>
    /// <param name="requestedPath">The current request path to validate.</param>
    /// <returns>The security context if valid and authorized; otherwise, null.</returns>
    Task<ApiClientContext?> ResolveKeyAsync(string keyHash, string requestedPath);
}
