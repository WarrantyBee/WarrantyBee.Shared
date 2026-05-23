using WarrantyBee.Shared.Core.Enums;

namespace WarrantyBee.Shared.Infrastructure.Abstractions;

/// <summary>
/// Defines a service for accessing the current user's security context.
/// </summary>
public interface ICurrentUserContext
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    long? UserId { get; }

    /// <summary>
    /// Gets the email address of the current user.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Gets the security role of the current user.
    /// </summary>
    SecurityRole Role { get; }

    /// <summary>
    /// Gets the set of permissions assigned to the current user.
    /// </summary>
    IEnumerable<SecurityPermission> Permissions { get; }
}
