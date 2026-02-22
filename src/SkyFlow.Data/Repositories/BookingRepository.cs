using Dapper;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IBookingRepository"/>.
/// </summary>
public class BookingRepository : IBookingRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookingRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public BookingRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Booking>> GetByFlightAsync(int flightId)
    {
        const string sql = "SELECT * FROM Bookings WHERE FlightId = @FlightId";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Booking>(sql, new { FlightId = flightId });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<dynamic>> GetManifestAsync(int flightId)
    {
        const string sql = @"
            SELECT 
                b.BookingId,
                p.FullName,
                p.PassportNumber,
                b.SeatNumber,
                b.Status
            FROM Bookings b
            INNER JOIN Passengers p ON b.PassengerId = p.PassengerId
            WHERE b.FlightId = @FlightId
            ORDER BY p.FullName";
            
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync(sql, new { FlightId = flightId });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Booking>> GetByPassengerAsync(int passengerId)
    {
        const string sql = "SELECT * FROM Bookings WHERE PassengerId = @PassengerId";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Booking>(sql, new { PassengerId = passengerId });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(Booking booking)
    {
        const string sql = @"
            INSERT INTO Bookings (PassengerId, FlightId, Status, SeatNumber)
            VALUES (@PassengerId, @FlightId, @Status, @SeatNumber);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            booking.PassengerId,
            booking.FlightId,
            Status = booking.Status.ToString(),
            booking.SeatNumber
        });
    }

    /// <inheritdoc />
    public async Task<bool> UpdateStatusAsync(int bookingId, string status)
    {
        const string sql = "UPDATE Bookings SET Status = @Status WHERE BookingId = @BookingId";
        using var connection = _context.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new { BookingId = bookingId, Status = status });
        return affected > 0;
    }
}
