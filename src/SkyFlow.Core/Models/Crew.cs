namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a crew member assigned by a user.
/// </summary>
public class Crew
{
    /// <summary>
    /// Gets or sets the unique identifier for the crew member.
    /// </summary>
    public int CrewId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the crew member.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the crew member's role (e.g., Pilot, Cabin Crew).
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who assigned this crew member.
    /// </summary>
    public int AssignedByUserId { get; set; }
}
