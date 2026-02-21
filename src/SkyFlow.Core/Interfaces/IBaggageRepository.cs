using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for baggage operations.
/// </summary>
public interface IBaggageRepository
{
    /// <summary>
    /// Retrieves all baggage items for a given booking.
    /// </summary>
    /// <param name="bookingId">The booking identifier.</param>
    /// <returns>A collection of baggage items.</returns>
    Task<IEnumerable<Baggage>> GetByBookingAsync(int bookingId);

    /// <summary>
    /// Creates a new baggage item.
    /// </summary>
    /// <param name="baggage">The baggage item to create.</param>
    /// <returns>The created baggage item's identifier.</returns>
    Task<int> CreateAsync(Baggage baggage);
}
