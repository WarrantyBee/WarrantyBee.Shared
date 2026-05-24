using System.Security.Cryptography;
using System.Text;
using WarrantyBee.Shared.Core.Enums;
using WarrantyBee.Shared.Infrastructure.Abstractions;
using WarrantyBee.Shared.Security.Abstractions;

namespace WarrantyBee.Shared.Security.Services;

/// <summary>
/// Implementation of <see cref="IApiKeyService"/> using hashing, database verification, and Redis caching.
/// </summary>
public class ApiKeyService : IApiKeyService
{
    private readonly IApiKeyRepository _keyRepository;
    private readonly ICacheService _cacheService;
    private readonly ITelemetryService _telemetry;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyService"/> class.
    /// </summary>
    public ApiKeyService(
        IApiKeyRepository keyRepository,
        ICacheService cacheService,
        ITelemetryService telemetry)
    {
        _keyRepository = keyRepository;
        _cacheService = cacheService;
        _telemetry = telemetry;
    }

    /// <summary>
    /// Validates the application credentials and ensures the requested path is allowed.
    /// </summary>
    public async Task<bool> ValidateAsync(string appId, string appSecret, string requestedPath)
    {
        if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(appSecret)) return false;

        var secretHash = ComputeHash(appSecret);
        var cacheKey = $"apikey:{appId}:{secretHash}:{requestedPath}";

        // 1. Check Cache
        var cachedResult = await _cacheService.GetAsync(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult == "1";
        }

        // 2. Check Database
        var isValid = await _keyRepository.ValidateAsync(appId, secretHash, requestedPath);

        // 3. Cache Result (5 minutes)
        await _cacheService.SetAsync(cacheKey, isValid ? "1" : "0", 300);

        if (!isValid)
        {
            _telemetry.Log(LogLevel.Warn, $"Invalid API access attempt. AppId: {appId}, Path: {requestedPath}");
        }

        return isValid;
    }

    /// <summary>
    /// Validates a standalone API key and ensures the requested path is allowed.
    /// </summary>
    public async Task<bool> ValidateKeyAsync(string apiKey, string requestedPath)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return false;

        var keyHash = ComputeHash(apiKey);
        var cacheKey = $"apikey:standalone:{keyHash}:{requestedPath}";

        // 1. Check Cache
        var cachedResult = await _cacheService.GetAsync(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult == "1";
        }

        // 2. Check Database
        var isValid = await _keyRepository.ValidateKeyAsync(keyHash, requestedPath);

        // 3. Cache Result (5 minutes)
        await _cacheService.SetAsync(cacheKey, isValid ? "1" : "0", 300);

        if (!isValid)
        {
            _telemetry.Log(LogLevel.Warn, $"Invalid standalone API Key access attempt. Path: {requestedPath}");
        }

        return isValid;
    }

    private string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }
}
