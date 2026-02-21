using SkyFlow.Core.Enums;

namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a booking linking a passenger to a flight with a status.
/// </summary>
public class Booking
{
    /// <summary>
    /// Gets or sets the unique identifier for the booking.
    /// </summary>
    public int BookingId { get; set; }

    /// <summary>
    /// Gets or sets the passenger identifier.
    /// </summary>
    public int PassengerId { get; set; }

    /// <summary>
    /// Gets or sets the flight identifier.
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// Gets or sets the current booking status.
    /// </summary>
    public BookingStatus Status { get; set; } = BookingStatus.Booked;

    /// <summary>
    /// Gets or sets the assigned seat number.
    /// </summary>
    public string? SeatNumber { get; set; }
}
