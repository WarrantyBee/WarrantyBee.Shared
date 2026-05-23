using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WarrantyBee.Shared.Infrastructure.Abstractions;

namespace WarrantyBee.Shared.Infrastructure.Persistence;

/// <summary>
/// Implementation of <see cref="IDbConnectionFactory"/> that creates SQL Server connections.
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration to read the connection string from.</param>
    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = Environment.GetEnvironmentVariable("WB__DB_CONN_STR") 
            ?? configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Database connection string is not configured.");
    }

    /// <summary>
    /// Creates a new <see cref="SqlConnection"/> instance.
    /// </summary>
    /// <returns>A new <see cref="IDbConnection"/>.</returns>
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
