namespace WarrantyBee.Shared.Security.Abstractions;

/// <summary>
/// Defines a repository for validating stateful API keys.
/// </summary>
public interface IApiKeyRepository
{
    /// <summary>
    /// Validates an AppId and secret hash against the database.
    /// </summary>
    /// <param name="appId">The unique Application ID.</param>
    /// <param name="secretHash">The hashed Application Secret.</param>
    /// <returns>True if valid and active; otherwise, false.</returns>
    Task<bool> ValidateAsync(string appId, string secretHash);
}
