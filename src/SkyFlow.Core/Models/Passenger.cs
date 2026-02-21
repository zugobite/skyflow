namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a passenger in the SkyFlow system.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Gets or sets the unique identifier for the passenger.
    /// </summary>
    public int PassengerId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the passenger.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the passport number.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national identity number.
    /// </summary>
    public string NationalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who created this passenger record.
    /// </summary>
    public int CreatedByUserId { get; set; }
}
