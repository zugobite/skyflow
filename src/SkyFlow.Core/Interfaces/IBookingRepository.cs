using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for booking operations.
/// </summary>
public interface IBookingRepository
{
    /// <summary>
    /// Retrieves all bookings for a given flight.
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    /// <returns>A collection of bookings for the flight.</returns>
    Task<IEnumerable<Booking>> GetByFlightAsync(int flightId);

    /// <summary>
    /// Retrieves the flight manifest (bookings with passenger details).
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    /// <returns>A collection of manifest records.</returns>
    Task<IEnumerable<dynamic>> GetManifestAsync(int flightId);

    /// <summary>
    /// Retrieves all bookings for a given passenger.
    /// </summary>
    /// <param name="passengerId">The passenger identifier.</param>
    /// <returns>A collection of bookings for the passenger.</returns>
    Task<IEnumerable<Booking>> GetByPassengerAsync(int passengerId);

    /// <summary>
    /// Creates a new booking.
    /// </summary>
    /// <param name="booking">The booking to create.</param>
    /// <returns>The created booking's identifier.</returns>
    Task<int> CreateAsync(Booking booking);

    /// <summary>
    /// Updates the status of a booking.
    /// </summary>
    /// <param name="bookingId">The booking identifier.</param>
    /// <param name="status">The new booking status.</param>
    /// <returns>True if the update was successful; false otherwise.</returns>
    Task<bool> UpdateStatusAsync(int bookingId, string status);
}
