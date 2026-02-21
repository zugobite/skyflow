using Dapper;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IAuditLogRepository"/>.
/// </summary>
public class AuditLogRepository : IAuditLogRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditLogRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public AuditLogRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AuditLog>> GetByUserAsync(int userId)
    {
        const string sql = "SELECT * FROM AuditLogs WHERE UserId = @UserId ORDER BY Timestamp DESC";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<AuditLog>(sql, new { UserId = userId });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(AuditLog auditLog)
    {
        const string sql = @"
            INSERT INTO AuditLogs (UserId, Action, EntityType, EntityId, Timestamp, Details)
            VALUES (@UserId, @Action, @EntityType, @EntityId, @Timestamp, @Details);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            auditLog.UserId,
            auditLog.Action,
            auditLog.EntityType,
            auditLog.EntityId,
            auditLog.Timestamp,
            auditLog.Details
        });
    }
}
