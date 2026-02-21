namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a baggage item associated with a booking.
/// </summary>
public class Baggage
{
    /// <summary>
    /// Gets or sets the unique identifier for the baggage item.
    /// </summary>
    public int BaggageId { get; set; }

    /// <summary>
    /// Gets or sets the booking identifier this baggage belongs to.
    /// </summary>
    public int BookingId { get; set; }

    /// <summary>
    /// Gets or sets the weight of the baggage in kilograms.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Gets or sets the unique baggage tag number.
    /// </summary>
    public string TagNumber { get; set; } = string.Empty;
}
