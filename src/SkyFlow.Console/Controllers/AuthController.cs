using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;
using SkyFlow.Console.Helpers;

namespace SkyFlow.Console.Controllers;

/// <summary>
/// Handles user authentication - login screen and credential verification.
/// </summary>
public class AuthController
{
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="auditLogRepository">The audit log repository.</param>
    public AuthController(IUserRepository userRepository, IAuditLogRepository auditLogRepository)
    {
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
    }

    /// <summary>
    /// Runs the login flow, prompting for credentials and verifying against the database.
    /// </summary>
    /// <returns>The authenticated user, or null if the user chose to exit.</returns>
    public async Task<User?> LoginAsync()
    {
        while (true)
        {
            ConsoleHelper.DisplayDivider("Login");

            var username = ConsoleHelper.Prompt("Username (or 'exit' to quit)");
            if (username.Equals("exit", StringComparison.OrdinalIgnoreCase))
                return null;

            var password = ConsoleHelper.PromptPassword("Password");

            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);

                if (user == null)
                {
                    ConsoleHelper.Error("Invalid username or password.");
                    ConsoleHelper.PressEnterToContinue();
                    continue;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    ConsoleHelper.Error("Invalid username or password.");
                    ConsoleHelper.PressEnterToContinue();
                    continue;
                }

                // Log successful login
                await _auditLogRepository.CreateAsync(new AuditLog
                {
                    UserId = user.UserId,
                    Action = "Login",
                    EntityType = "User",
                    EntityId = user.UserId,
                    Details = $"User '{user.Username}' logged in successfully."
                });

                ConsoleHelper.Success($"Welcome, {user.FullName}! ({user.Role})");
                return user;
            }
            catch (Exception ex)
            {
                ConsoleHelper.Error($"An error occurred: {ex.Message}");
                ConsoleHelper.PressEnterToContinue();
            }
        }
    }
}
