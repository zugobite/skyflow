using SkyFlow.Core.Enums;

namespace SkyFlow.Core.Models;

/// <summary>
/// Abstract base class for all users in the SkyFlow system.
/// Demonstrates inheritance - derived classes must override <see cref="DisplayDashboard"/>.
/// </summary>
public abstract class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username used for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the BCrypt-hashed password.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Displays the role-specific dashboard menu.
    /// Demonstrates polymorphism - each derived class provides its own implementation.
    /// </summary>
    public abstract void DisplayDashboard();
}
