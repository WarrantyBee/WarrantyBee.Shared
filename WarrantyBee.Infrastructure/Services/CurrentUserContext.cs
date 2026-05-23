using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WarrantyBee.Shared.Infrastructure.Abstractions;
using WarrantyBee.Shared.Core.Enums;

namespace WarrantyBee.Shared.Infrastructure.Services;

/// <summary>
/// Provides access to the current authenticated user's context using <see cref="IHttpContextAccessor"/>.
/// </summary>
public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserContext"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId
    {
        get
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");
            return long.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) 
                           ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("email");

    public SecurityRole Role
    {
        get
        {
            var roleStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("role");
            return Enum.TryParse<SecurityRole>(roleStr, true, out var role) ? role : SecurityRole.None;
        }
    }

    public IEnumerable<SecurityPermission> Permissions
    {
        get
        {
            var permissionsStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue("permissions");
            if (string.IsNullOrWhiteSpace(permissionsStr)) return [];
            return permissionsStr.Split(',')
                .Select(p => Enum.TryParse<SecurityPermission>(p.Trim(), true, out var perm) ? perm : SecurityPermission.None)
                .Where(p => p != SecurityPermission.None);
        }
    }
}
