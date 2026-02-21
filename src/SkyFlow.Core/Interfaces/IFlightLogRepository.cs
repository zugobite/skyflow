using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for flight log operations.
/// </summary>
public interface IFlightLogRepository
{
    /// <summary>
    /// Retrieves all log entries for a given flight.
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    /// <returns>A collection of flight log entries.</returns>
    Task<IEnumerable<FlightLog>> GetByFlightAsync(int flightId);

    /// <summary>
    /// Creates a new flight log entry.
    /// </summary>
    /// <param name="flightLog">The flight log entry to create.</param>
    /// <returns>The created flight log entry's identifier.</returns>
    Task<int> CreateAsync(FlightLog flightLog);
}
