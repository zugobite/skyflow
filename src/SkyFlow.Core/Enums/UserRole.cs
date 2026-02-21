namespace SkyFlow.Core.Enums;

/// <summary>
/// Represents the role of a user in the SkyFlow system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Administrator with full system access including flight and staff management.
    /// </summary>
    Admin,

    /// <summary>
    /// Gate agent with passenger check-in and boarding capabilities.
    /// </summary>
    GateAgent
}
