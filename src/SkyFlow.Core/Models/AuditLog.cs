namespace SkyFlow.Core.Models;

/// <summary>
/// Represents an audit log entry tracking user actions on entities.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log entry.
    /// </summary>
    public int AuditLogId { get; set; }

    /// <summary>
    /// Gets or sets the user who performed the action.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the action that was performed (e.g., "Create", "Update", "Delete").
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of entity affected (e.g., "Flight", "Booking").
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the affected entity.
    /// </summary>
    public int EntityId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the action.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional details about the action.
    /// </summary>
    public string? Details { get; set; }
}
