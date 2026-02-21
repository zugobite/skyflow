using SkyFlow.Core.Enums;

namespace SkyFlow.Core.Models;

/// <summary>
/// Represents a gate agent user with passenger check-in and boarding capabilities.
/// Inherits from <see cref="User"/> and overrides <see cref="DisplayDashboard"/>.
/// </summary>
public class GateAgent : User
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GateAgent"/> class with the GateAgent role.
    /// </summary>
    public GateAgent()
    {
        Role = UserRole.GateAgent;
    }

    /// <summary>
    /// Displays the gate agent-specific dashboard menu.
    /// </summary>
    public override void DisplayDashboard()
    {
        // Implementation will be provided by GateAgentDashboard controller in SkyFlow.Console
    }
}
