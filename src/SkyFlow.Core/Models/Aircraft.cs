namespace SkyFlow.Core.Models;

/// <summary>
/// Represents an aircraft with registration details and passenger capacity.
/// </summary>
public class Aircraft
{
    /// <summary>
    /// Gets or sets the unique identifier for the aircraft.
    /// </summary>
    public int AircraftId { get; set; }

    /// <summary>
    /// Gets or sets the aircraft registration number (e.g., ZS-ABC).
    /// </summary>
    public string RegistrationNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the aircraft model (e.g., Boeing 737-800).
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the maximum passenger capacity.
    /// </summary>
    public int Capacity { get; set; }
}
