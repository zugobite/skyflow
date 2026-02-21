using Dapper;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IPassengerRepository"/>.
/// </summary>
public class PassengerRepository : IPassengerRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PassengerRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public PassengerRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Passenger>> GetByFlightAsync(int flightId)
    {
        const string sql = @"
            SELECT p.*
            FROM Passengers p
            INNER JOIN Bookings b ON p.PassengerId = b.PassengerId
            WHERE b.FlightId = @FlightId";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Passenger>(sql, new { FlightId = flightId });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Passenger>> SearchByIdOrPassportAsync(string searchTerm)
    {
        const string sql = @"
            SELECT * FROM Passengers
            WHERE NationalId = @SearchTerm OR PassportNumber = @SearchTerm";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Passenger>(sql, new { SearchTerm = searchTerm });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(Passenger passenger)
    {
        const string sql = @"
            INSERT INTO Passengers (FullName, PassportNumber, NationalId, CreatedByUserId)
            VALUES (@FullName, @PassportNumber, @NationalId, @CreatedByUserId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            passenger.FullName,
            passenger.PassportNumber,
            passenger.NationalId,
            passenger.CreatedByUserId
        });
    }
}
