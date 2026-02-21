using Dapper;
using SkyFlow.Core.Enums;
using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;

namespace SkyFlow.Data.Repositories;

/// <summary>
/// Dapper-based implementation of <see cref="IUserRepository"/>.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The Dapper context for database connections.</param>
    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = "SELECT * FROM Users WHERE Username = @Username";
        using var connection = _context.CreateConnection();
        var row = await connection.QueryFirstOrDefaultAsync(sql, new { Username = username });

        if (row == null) return null;

        return MapToUser(row);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Users";
        using var connection = _context.CreateConnection();
        var rows = await connection.QueryAsync(sql);

        return rows.Select(MapToUser).ToList();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Username, PasswordHash, Role, FullName, Email)
            VALUES (@Username, @PasswordHash, @Role, @FullName, @Email);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new
        {
            user.Username,
            user.PasswordHash,
            Role = user.Role.ToString(),
            user.FullName,
            user.Email
        });
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int userId)
    {
        const string sql = "DELETE FROM Users WHERE UserId = @UserId";
        using var connection = _context.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new { UserId = userId });
        return affected > 0;
    }

    /// <summary>
    /// Maps a dynamic database row to the appropriate User subclass.
    /// </summary>
    private static User MapToUser(dynamic row)
    {
        var role = Enum.Parse<UserRole>((string)row.Role);
        User user = role switch
        {
            UserRole.Admin => new Admin(),
            UserRole.GateAgent => new GateAgent(),
            _ => throw new InvalidOperationException($"Unknown role: {row.Role}")
        };

        user.UserId = (int)row.UserId;
        user.Username = (string)row.Username;
        user.PasswordHash = (string)row.PasswordHash;
        user.Role = role;
        user.FullName = (string)row.FullName;
        user.Email = (string)row.Email;

        return user;
    }
}
