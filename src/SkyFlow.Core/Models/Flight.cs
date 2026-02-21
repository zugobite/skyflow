using SkyFlow.Core.Enums;

namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a flight with encapsulated status transitions.
/// Demonstrates encapsulation - status can only be changed via transition methods.
/// </summary>
public class Flight
{
    private FlightStatus _status = FlightStatus.Scheduled;

    /// <summary>
    /// Gets or sets the unique identifier for the flight.
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// Gets or sets the flight number (e.g., SF101).
    /// </summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the origin airport identifier.
    /// </summary>
    public int OriginAirportId { get; set; }

    /// <summary>
    /// Gets or sets the destination airport identifier.
    /// </summary>
    public int DestinationAirportId { get; set; }

    /// <summary>
    /// Gets or sets the scheduled departure time.
    /// </summary>
    public DateTime DepartureTime { get; set; }

    /// <summary>
    /// Gets the current flight status. Can only be changed via transition methods.
    /// </summary>
    public FlightStatus Status
    {
        get => _status;
        set => _status = value; // Setter kept for Dapper mapping; use transition methods in business logic
    }

    /// <summary>
    /// Gets or sets the assigned gate agent's user identifier.
    /// </summary>
    public int? GateAgentId { get; set; }

    /// <summary>
    /// Transitions the flight status to Boarding.
    /// </summary>
    /// <returns>True if the transition was valid; false otherwise.</returns>
    public bool BeginBoarding()
    {
        if (_status != FlightStatus.Scheduled)
            return false;

        _status = FlightStatus.Boarding;
        return true;
    }

    /// <summary>
    /// Transitions the flight status to Departed.
    /// </summary>
    /// <returns>True if the transition was valid; false otherwise.</returns>
    public bool DepartFlight()
    {
        if (_status != FlightStatus.Boarding)
            return false;

        _status = FlightStatus.Departed;
        return true;
    }

    /// <summary>
    /// Transitions the flight status to Cancelled.
    /// </summary>
    /// <returns>True if the transition was valid; false otherwise.</returns>
    public bool CancelFlight()
    {
        if (_status == FlightStatus.Departed || _status == FlightStatus.Cancelled)
            return false;

        _status = FlightStatus.Cancelled;
        return true;
    }
}
