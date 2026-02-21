namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a log entry for flight status changes.
/// </summary>
public class FlightLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the log entry.
    /// </summary>
    public int LogId { get; set; }

    /// <summary>
    /// Gets or sets the flight identifier.
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// Gets or sets the action that was performed (e.g., "StatusChanged").
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the action.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional details about the action.
    /// </summary>
    public string? Details { get; set; }
}
