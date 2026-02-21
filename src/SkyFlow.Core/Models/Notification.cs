namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a notification sent to a user.
/// </summary>
public class Notification
{
    /// <summary>
    /// Gets or sets the unique identifier for the notification.
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Gets or sets the target user identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the notification message content.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the notification has been read.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
