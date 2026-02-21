using Dapper;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IFlightLogRepository"/>.
/// </summary>
public class FlightLogRepository : IFlightLogRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlightLogRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public FlightLogRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FlightLog>> GetByFlightAsync(int flightId)
    {
        const string sql = "SELECT * FROM FlightLogs WHERE FlightId = @FlightId ORDER BY Timestamp DESC";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<FlightLog>(sql, new { FlightId = flightId });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(FlightLog flightLog)
    {
        const string sql = @"
            INSERT INTO FlightLogs (FlightId, Action, Timestamp, Details)
            VALUES (@FlightId, @Action, @Timestamp, @Details);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            flightLog.FlightId,
            flightLog.Action,
            flightLog.Timestamp,
            flightLog.Details
        });
    }
}
