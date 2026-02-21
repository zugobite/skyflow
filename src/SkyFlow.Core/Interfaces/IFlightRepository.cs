using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for flight operations.
/// </summary>
public interface IFlightRepository
{
    /// <summary>
    /// Retrieves all flights.
    /// </summary>
    /// <returns>A collection of all flights.</returns>
    Task<IEnumerable<Flight>> GetAllAsync();

    /// <summary>
    /// Retrieves a flight by its identifier.
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    /// <returns>The matching flight, or null if not found.</returns>
    Task<Flight?> GetByIdAsync(int flightId);

    /// <summary>
    /// Creates a new flight.
    /// </summary>
    /// <param name="flight">The flight to create.</param>
    /// <returns>The created flight's identifier.</returns>
    Task<int> CreateAsync(Flight flight);

    /// <summary>
    /// Updates the status of a flight.
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    /// <param name="status">The new status value.</param>
    /// <returns>True if the update was successful; false otherwise.</returns>
    Task<bool> UpdateStatusAsync(int flightId, string status);

    /// <summary>
    /// Retrieves all flights with their current booking count and aircraft capacity.
    /// </summary>
    /// <returns>A collection of flights with occupancy data.</returns>
    Task<IEnumerable<dynamic>> GetFlightsWithOccupancyAsync();
}
