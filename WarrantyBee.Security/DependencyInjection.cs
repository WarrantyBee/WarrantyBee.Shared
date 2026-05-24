using Microsoft.Extensions.DependencyInjection;
using WarrantyBee.Shared.Security.Abstractions;
using WarrantyBee.Shared.Security.Services;
using WarrantyBee.Shared.Security.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarrantyBee.Shared.Core.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using WarrantyBee.Shared.Core.Enums;

namespace WarrantyBee.Shared.Security;

/// <summary>
/// Provides extension methods for registering security-related services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers unified security protocols, including API Key and JWT authentication.
    /// </summary>
    public static IServiceCollection AddWarrantyBeeSecurity(this IServiceCollection services)
    {
        services.AddScoped<IApiKeyService, ApiKeyService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var sp = services.BuildServiceProvider();
            var config = sp.GetRequiredService<IOptions<AppConfiguration>>().Value.Jwt;
            
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config?.Issuer,
                ValidAudience = config?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config?.Secret ?? ""))
            };
        })
        .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, null);

        return services;
    }

    /// <summary>
    /// Registers granular authorization policies based on the SecurityPermission enum.
    /// </summary>
    public static IServiceCollection AddWarrantyBeeAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Fallback: Default policy requires any authenticated identity
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationOptions.DefaultScheme)
                .Build();

            // Register a policy for every granular permission
            foreach (var permission in Enum.GetValues<SecurityPermission>())
            {
                if (permission == SecurityPermission.None) continue;
                
                options.AddPolicy($"Permission.{permission}", policy =>
                    policy.RequireAuthenticatedUser()
                          .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationOptions.DefaultScheme)
                          .RequireClaim("permission", permission.ToString()));
            }
        });

        return services;
    }
}
