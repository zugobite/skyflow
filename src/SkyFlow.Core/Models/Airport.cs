namespace SkyFlow.Core.Models;

/// <summary>
/// Represents an airport with IATA code and location details.
/// </summary>
public class Airport
{
    /// <summary>
    /// Gets or sets the unique identifier for the airport.
    /// </summary>
    public int AirportId { get; set; }

    /// <summary>
    /// Gets or sets the IATA airport code (e.g., JNB, CPT).
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the airport.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city where the airport is located.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country where the airport is located.
    /// </summary>
    public string Country { get; set; } = string.Empty;
}
