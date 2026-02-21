namespace SkyFlow.Core.Enums;

/// <summary>
/// Represents the current status of a passenger booking.
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Passenger has a confirmed booking but has not checked in.
    /// </summary>
    Booked,

    /// <summary>
    /// Passenger has completed the check-in process.
    /// </summary>
    CheckedIn,

    /// <summary>
    /// Passenger has boarded the aircraft.
    /// </summary>
    Boarded
}
