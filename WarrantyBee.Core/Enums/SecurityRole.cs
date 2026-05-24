namespace WarrantyBee.Shared.Core.Enums;

/// <summary>
/// Represents the security roles available in the system, organized by Platform, Business, Network, and Consumer tiers.
/// </summary>
public enum SecurityRole
{
    /// <summary>
    /// No role specified.
    /// </summary>
    None = 0,

    // TIER 1: Platform Level
    /// <summary>
    /// Ultimate platform administrator with access to all tenants and system configurations.
    /// </summary>
    PlatformAdmin = 1,
    /// <summary>
    /// System support and technical auditor for the platform.
    /// </summary>
    PlatformSupport = 2,

    // TIER 2: Business Level (Brand/OEM)
    /// <summary>
    /// The primary owner of a business tenant.
    /// </summary>
    BusinessOwner = 3,
    /// <summary>
    /// Administrator for a specific business tenant.
    /// </summary>
    BusinessAdmin = 4,
    /// <summary>
    /// Operations manager focusing on logistics and trends for the brand.
    /// </summary>
    Planner = 5,
    /// <summary>
    /// Frontline support agent for the brand.
    /// </summary>
    BrandSupport = 6,

    // TIER 3: Network Level (Supply & Service)
    /// <summary>
    /// Bulk buyer and stock manager for a brand.
    /// </summary>
    Distributor = 7,
    /// <summary>
    /// Front-facing seller who activates warranties upon purchase.
    /// </summary>
    Retailer = 8,
    /// <summary>
    /// Manager of an authorized service center who dispatches technicians.
    /// </summary>
    ServiceCenterAdmin = 9,
    /// <summary>
    /// Field agent responsible for performing appliance repairs.
    /// </summary>
    Technician = 10,

    // TIER 4: Consumer Level
    /// <summary>
    /// End-user who owns products and initiates claims.
    /// </summary>
    Customer = 11
}
