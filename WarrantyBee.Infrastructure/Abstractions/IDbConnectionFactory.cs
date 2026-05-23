using System.Data;

namespace WarrantyBee.Shared.Infrastructure.Abstractions;

/// <summary>
/// Defines a factory for creating database connections.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    /// <returns>An <see cref="IDbConnection"/> instance.</returns>
    IDbConnection CreateConnection();
}
