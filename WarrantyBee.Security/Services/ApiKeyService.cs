using System.Security.Cryptography;
using System.Text;
using WarrantyBee.Shared.Core.Enums;
using WarrantyBee.Shared.Infrastructure.Abstractions;
using WarrantyBee.Shared.Security.Abstractions;

namespace WarrantyBee.Shared.Security.Services;

/// <summary>
/// Implementation of <see cref="IApiKeyService"/> that unifies API Key validation and context resolution.
/// </summary>
public class ApiKeyService : IApiKeyService
{
    private readonly IApiKeyRepository _keyRepository;
    private readonly ICacheService _cacheService;
    private readonly ITelemetryService _telemetry;

    public ApiKeyService(
        IApiKeyRepository keyRepository,
        ICacheService cacheService,
        ITelemetryService telemetry)
    {
        _keyRepository = keyRepository;
        _cacheService = cacheService;
        _telemetry = telemetry;
    }

    public async Task<ApiClientContext?> ValidateAsync(string appId, string appSecret, string requestedPath)
    {
        if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(appSecret)) return null;

        var secretHash = ComputeHash(appSecret);
        // Resolve from repository (which checks DB + endpoints)
        var context = await _keyRepository.ResolveAsync(appId, secretHash, requestedPath);

        if (context == null)
        {
            _telemetry.Log(LogLevel.Warn, $"Invalid API access attempt. AppId: {appId}, Path: {requestedPath}");
        }

        return context;
    }

    public async Task<ApiClientContext?> ValidateKeyAsync(string apiKey, string requestedPath)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return null;

        var keyHash = ComputeHash(apiKey);
        var context = await _keyRepository.ResolveKeyAsync(keyHash, requestedPath);

        if (context == null)
        {
            _telemetry.Log(LogLevel.Warn, $"Invalid standalone API Key access attempt. Path: {requestedPath}");
        }

        return context;
    }

    private string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }
}
