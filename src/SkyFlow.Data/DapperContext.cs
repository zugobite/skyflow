using System.Data;
using Microsoft.Data.SqlClient;

namespace SkyFlow.Data;

/// <summary>
/// Provides SQL Server database connections for Dapper-based repositories.
/// </summary>
public class DapperContext
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DapperContext"/> class.
    /// </summary>
    /// <param name="connectionString">The SQL Server connection string.</param>
    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Creates and returns a new SQL Server database connection.
    /// </summary>
    /// <returns>An <see cref="IDbConnection"/> instance.</returns>
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
