using Dapper;
using SkyFlow.Core.Enums;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IFlightRepository"/>.
/// </summary>
public class FlightRepository : IFlightRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlightRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public FlightRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Flight>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Flights";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Flight>(sql);
    }

    /// <inheritdoc />
    public async Task<Flight?> GetByIdAsync(int flightId)
    {
        const string sql = "SELECT * FROM Flights WHERE FlightId = @FlightId";
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Flight>(sql, new { FlightId = flightId });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(Flight flight)
    {
        const string sql = @"
            INSERT INTO Flights (FlightNumber, OriginAirportId, DestinationAirportId, DepartureTime, Status, GateAgentId)
            VALUES (@FlightNumber, @OriginAirportId, @DestinationAirportId, @DepartureTime, @Status, @GateAgentId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            flight.FlightNumber,
            flight.OriginAirportId,
            flight.DestinationAirportId,
            flight.DepartureTime,
            Status = flight.Status.ToString(),
            flight.GateAgentId
        });
    }

    /// <inheritdoc />
    public async Task<int> CreateFlightWithAircraftAsync(Flight flight, Aircraft aircraft)
    {
        using var connection = _context.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Create Aircraft
            const string aircraftSql = @"
                INSERT INTO Aircraft (RegistrationNo, Model, Capacity)
                VALUES (@RegistrationNo, @Model, @Capacity);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            
            var aircraftId = await connection.QuerySingleAsync<int>(aircraftSql, new
            {
                aircraft.RegistrationNo,
                aircraft.Model,
                aircraft.Capacity
            }, transaction);

            // 2. Create Flight
            const string flightSql = @"
                INSERT INTO Flights (FlightNumber, OriginAirportId, DestinationAirportId, DepartureTime, Status, GateAgentId)
                VALUES (@FlightNumber, @OriginAirportId, @DestinationAirportId, @DepartureTime, @Status, @GateAgentId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            
            var flightId = await connection.QuerySingleAsync<int>(flightSql, new
            {
                flight.FlightNumber,
                flight.OriginAirportId,
                flight.DestinationAirportId,
                flight.DepartureTime,
                Status = flight.Status.ToString(),
                flight.GateAgentId
            }, transaction);

            // 3. Create FlightAssignment
            const string assignmentSql = @"
                INSERT INTO FlightAssignments (FlightId, AircraftId)
                VALUES (@FlightId, @AircraftId);";
            
            await connection.ExecuteAsync(assignmentSql, new
            {
                FlightId = flightId,
                AircraftId = aircraftId
            }, transaction);

            transaction.Commit();
            return flightId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateStatusAsync(int flightId, string status)
    {
        const string sql = "UPDATE Flights SET Status = @Status WHERE FlightId = @FlightId";
        using var connection = _context.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new { FlightId = flightId, Status = status });
        return affected > 0;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<dynamic>> GetFlightsWithOccupancyAsync()
    {
        const string sql = @"
            SELECT
                f.FlightId,
                f.FlightNumber,
                ao.Code AS Origin,
                ad.Code AS Destination,
                f.DepartureTime,
                f.Status,
                a.Capacity,
                COUNT(b.BookingId) AS BookingCount
            FROM Flights f
            LEFT JOIN FlightAssignments fa ON f.FlightId = fa.FlightId
            LEFT JOIN Aircraft a ON fa.AircraftId = a.AircraftId
            LEFT JOIN Airports ao ON f.OriginAirportId = ao.AirportId
            LEFT JOIN Airports ad ON f.DestinationAirportId = ad.AirportId
            LEFT JOIN Bookings b ON f.FlightId = b.FlightId
            GROUP BY f.FlightId, f.FlightNumber, ao.Code, ad.Code,
                     f.DepartureTime, f.Status, a.Capacity";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync(sql);
    }
}
