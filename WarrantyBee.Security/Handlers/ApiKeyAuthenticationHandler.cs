using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WarrantyBee.Shared.Security.Abstractions;

namespace WarrantyBee.Shared.Security.Handlers;

/// <summary>
/// Options for API Key authentication.
/// </summary>
public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
    public string Scheme => DefaultScheme;
}

/// <summary>
/// Custom authentication handler for validating API keys and populating the <see cref="ClaimsPrincipal"/>.
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyService apiKeyService) : base(options, logger, encoder)
    {
        _apiKeyService = apiKeyService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var request = Context.Request;
        var requestedPath = request.Path.Value ?? string.Empty;
        ApiClientContext? apiClientContext = null;

        // 1. Try x-api-key header
        if (request.Headers.TryGetValue("x-api-key", out var apiKey))
        {
            apiClientContext = await _apiKeyService.ValidateKeyAsync(apiKey!, requestedPath);
        }
        // 2. Try X-APP-ID / X-APP-SECRET pair
        else if (request.Headers.TryGetValue("X-APP-ID", out var appId) &&
                 request.Headers.TryGetValue("X-APP-SECRET", out var appSecret))
        {
            apiClientContext = await _apiKeyService.ValidateAsync(appId!, appSecret!, requestedPath);
        }

        if (apiClientContext == null)
        {
            return AuthenticateResult.NoResult();
        }

        // 3. Build Claims
        var claims = new List<Claim>
        {
            new Claim("appId", apiClientContext.AppId),
            new Claim(ClaimTypes.Name, apiClientContext.Name),
            new Claim(ClaimTypes.Role, apiClientContext.Role.ToString())
        };

        // Add granular permissions as claims
        foreach (var permission in apiClientContext.Permissions)
        {
            claims.Add(new Claim("permission", permission.ToString()));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
