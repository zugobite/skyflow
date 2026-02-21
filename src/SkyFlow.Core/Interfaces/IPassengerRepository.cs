using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for passenger operations.
/// </summary>
public interface IPassengerRepository
{
    /// <summary>
    /// Retrieves all passengers for a given flight.
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    /// <returns>A collection of passengers booked on the flight.</returns>
    Task<IEnumerable<Passenger>> GetByFlightAsync(int flightId);

    /// <summary>
    /// Searches for passengers by national ID or passport number.
    /// </summary>
    /// <param name="searchTerm">The national ID or passport number to search for.</param>
    /// <returns>A collection of matching passengers.</returns>
    Task<IEnumerable<Passenger>> SearchByIdOrPassportAsync(string searchTerm);

    /// <summary>
    /// Creates a new passenger.
    /// </summary>
    /// <param name="passenger">The passenger to create.</param>
    /// <returns>The created passenger's identifier.</returns>
    Task<int> CreateAsync(Passenger passenger);
}
