using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for audit log operations.
/// </summary>
public interface IAuditLogRepository
{
    /// <summary>
    /// Retrieves all audit log entries for a given user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of audit log entries.</returns>
    Task<IEnumerable<AuditLog>> GetByUserAsync(int userId);

    /// <summary>
    /// Retrieves all audit log entries.
    /// </summary>
    /// <returns>A collection of all audit log entries.</returns>
    Task<IEnumerable<AuditLog>> GetAllAsync();

    /// <summary>
    /// Creates a new audit log entry.
    /// </summary>
    /// <param name="auditLog">The audit log entry to create.</param>
    /// <returns>The created audit log entry's identifier.</returns>
    Task<int> CreateAsync(AuditLog auditLog);
}
