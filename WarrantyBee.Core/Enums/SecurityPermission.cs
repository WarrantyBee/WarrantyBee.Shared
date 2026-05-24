namespace WarrantyBee.Shared.Core.Enums;

/// <summary>
/// Represents granular security permissions available in the system.
/// </summary>
public enum SecurityPermission
{
    /// <summary>
    /// No permission.
    /// </summary>
    None = 0,

    // Profile Management
    /// <summary>
    /// Permission to edit a user profile.
    /// </summary>
    EditProfile = 1,
    /// <summary>
    /// Permission to change an avatar.
    /// </summary>
    ChangeAvatar = 2,
    /// <summary>
    /// Permission to access a user profile.
    /// </summary>
    AccessProfile = 3,

    // Platform Level
    /// <summary>
    /// Full management of the platform and tenants.
    /// </summary>
    ManagePlatform = 10,
    /// <summary>
    /// Technical auditing and log access.
    /// </summary>
    AuditSystem = 11,

    // Business Level
    /// <summary>
    /// Manage users within a specific business tenant.
    /// </summary>
    ManageBusinessUsers = 20,
    /// <summary>
    /// Manage the product catalog and warranty policies.
    /// </summary>
    ManageProducts = 21,
    /// <summary>
    /// Analyze trends and manage spare parts inventory.
    /// </summary>
    ManageLogistics = 22,
    
    // Claim & Service Management
    /// <summary>
    /// Final approval or rejection of high-value warranty claims.
    /// </summary>
    ApproveClaims = 30,
    /// <summary>
    /// Dispatch tickets to service centers and technicians.
    /// </summary>
    AssignTickets = 31,
    /// <summary>
    /// Update ticket status and repair progress.
    /// </summary>
    UpdateTickets = 32,
    /// <summary>
    /// Initiate a new warranty claim.
    /// </summary>
    SubmitClaims = 33,
    
    // Supply Chain
    /// <summary>
    /// Activate a product warranty at the point of sale.
    /// </summary>
    ActivateWarranty = 40,
    /// <summary>
    /// Manage batch transfers and stock allocations.
    /// </summary>
    ManageInventory = 41
}
