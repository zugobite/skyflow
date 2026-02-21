using Dapper;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IBaggageRepository"/>.
/// </summary>
public class BaggageRepository : IBaggageRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaggageRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public BaggageRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Baggage>> GetByBookingAsync(int bookingId)
    {
        const string sql = "SELECT * FROM Baggage WHERE BookingId = @BookingId";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Baggage>(sql, new { BookingId = bookingId });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(Baggage baggage)
    {
        const string sql = @"
            INSERT INTO Baggage (BookingId, Weight, TagNumber)
            VALUES (@BookingId, @Weight, @TagNumber);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            baggage.BookingId,
            baggage.Weight,
            baggage.TagNumber
        });
    }
}
