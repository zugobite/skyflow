namespace SkyFlow.Core.Models;

/// <summary>
/// Represents the one-to-one assignment of an aircraft to a flight.
/// </summary>
public class FlightAssignment
{
    /// <summary>
    /// Gets or sets the unique identifier for the flight assignment.
    /// </summary>
    public int FlightAssignmentId { get; set; }

    /// <summary>
    /// Gets or sets the flight identifier (unique - one assignment per flight).
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// Gets or sets the assigned aircraft identifier.
    /// </summary>
    public int AircraftId { get; set; }
}
