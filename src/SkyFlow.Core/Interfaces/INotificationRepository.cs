using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for notification operations.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Retrieves all notifications for a given user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of notifications.</returns>
    Task<IEnumerable<Notification>> GetByUserAsync(int userId);

    /// <summary>
    /// Retrieves the count of unread notifications for a given user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The count of unread notifications.</returns>
    Task<int> GetUnreadCountAsync(int userId);

    /// <summary>
    /// Creates a new notification.
    /// </summary>
    /// <param name="notification">The notification to create.</param>
    /// <returns>The created notification's identifier.</returns>
    Task<int> CreateAsync(Notification notification);

    /// <summary>
    /// Marks a notification as read.
    /// </summary>
    /// <param name="notificationId">The notification identifier.</param>
    /// <returns>True if the update was successful; false otherwise.</returns>
    Task<bool> MarkAsReadAsync(int notificationId);
}
