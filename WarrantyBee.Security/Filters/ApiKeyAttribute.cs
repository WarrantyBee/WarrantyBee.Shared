using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using WarrantyBee.Shared.Security.Abstractions;

namespace WarrantyBee.Shared.Security.Filters;

/// <summary>
/// Filter that validates API credentials (X-APP-ID/X-APP-SECRET or x-api-key) and restricts access to mapped endpoints.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string AppIdHeaderName = "X-APP-ID";
    private const string AppSecretHeaderName = "X-APP-SECRET";
    private const string ApiKeyHeaderName = "x-api-key";

    /// <summary>
    /// Validates the API credentials and endpoint permissions before executing the action.
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        var requestedPath = request.Path.Value ?? string.Empty;
        var apiKeyService = context.HttpContext.RequestServices.GetRequiredService<IApiKeyService>();
        bool isValid = false;

        // 1. Check for standalone API Key
        if (request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            isValid = await apiKeyService.ValidateKeyAsync(apiKey!, requestedPath);
        }
        // 2. Check for App ID + App Secret pair
        else if (request.Headers.TryGetValue(AppIdHeaderName, out var appId) &&
                 request.Headers.TryGetValue(AppSecretHeaderName, out var appSecret))
        {
            isValid = await apiKeyService.ValidateAsync(appId!, appSecret!, requestedPath);
        }
        else
        {
            context.Result = new UnauthorizedObjectResult("API credentials missing. Provide 'x-api-key' or 'X-APP-ID'/'X-APP-SECRET' headers.");
            return;
        }

        if (!isValid)
        {
            context.Result = new ObjectResult("Invalid API credentials or unauthorized endpoint access.") { StatusCode = 403 };
            return;
        }

        await next();
    }
}
