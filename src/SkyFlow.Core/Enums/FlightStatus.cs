namespace SkyFlow.Core.Enums;

/// <summary>
/// Represents the current status of a flight.
/// </summary>
public enum FlightStatus
{
    /// <summary>
    /// Flight is scheduled but has not yet begun boarding.
    /// </summary>
    Scheduled,

    /// <summary>
    /// Flight is currently boarding passengers.
    /// </summary>
    Boarding,

    /// <summary>
    /// Flight has departed.
    /// </summary>
    Departed,

    /// <summary>
    /// Flight has been cancelled.
    /// </summary>
    Cancelled
}
