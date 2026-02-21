using SkyFlow.Core.Enums;

namespace SkyFlow.Core.Models;

/// <summary>
/// Represents an administrator user with flight and staff management capabilities.
/// Inherits from <see cref="User"/> and overrides <see cref="DisplayDashboard"/>.
/// </summary>
public class Admin : User
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Admin"/> class with the Admin role.
    /// </summary>
    public Admin()
    {
        Role = UserRole.Admin;
    }

    /// <summary>
    /// Displays the admin-specific dashboard menu.
    /// </summary>
    public override void DisplayDashboard()
    {
        // Implementation will be provided by AdminDashboard controller in SkyFlow.Console
    }
}
