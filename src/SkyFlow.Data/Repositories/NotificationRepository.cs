using Dapper;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="INotificationRepository"/>.
/// </summary>
public class NotificationRepository : INotificationRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public NotificationRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Notification>> GetByUserAsync(int userId)
    {
        const string sql = "SELECT * FROM Notifications WHERE UserId = @UserId ORDER BY CreatedAt DESC";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Notification>(sql, new { UserId = userId });
    }

    /// <inheritdoc />
    public async Task<int> GetUnreadCountAsync(int userId)
    {
        const string sql = "SELECT COUNT(*) FROM Notifications WHERE UserId = @UserId AND IsRead = 0";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new { UserId = userId });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(Notification notification)
    {
        const string sql = @"
            INSERT INTO Notifications (UserId, Message, IsRead, CreatedAt)
            VALUES (@UserId, @Message, @IsRead, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            notification.UserId,
            notification.Message,
            notification.IsRead,
            notification.CreatedAt
        });
    }

    /// <inheritdoc />
    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        const string sql = "UPDATE Notifications SET IsRead = 1 WHERE NotificationId = @NotificationId";
        using var connection = _context.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new { NotificationId = notificationId });
        return affected > 0;
    }
}
