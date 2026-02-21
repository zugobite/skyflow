using SkyFlow.Core.Models;

namespace SkyFlow.Core.Interfaces;

/// <summary>
/// Defines the data access contract for user operations.
/// Demonstrates abstraction - separates database logic from business logic.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The matching user, or null if not found.</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>The created user's identifier.</returns>
    Task<int> CreateAsync(User user);

    /// <summary>
    /// Deletes a user by their identifier.
    /// </summary>
    /// <param name="userId">The identifier of the user to delete.</param>
    /// <returns>True if the user was deleted; false otherwise.</returns>
    Task<bool> DeleteAsync(int userId);
}
