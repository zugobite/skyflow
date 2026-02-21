using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;
using SkyFlow.Console.Helpers;

namespace SkyFlow.Console.Controllers;

/// <summary>
/// Admin-specific dashboard handling flight management, system oversight, and staff management.
/// </summary>
public class AdminDashboard
{
    private readonly IFlightRepository _flightRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminDashboard"/> class.
    /// </summary>
    public AdminDashboard(
        IFlightRepository flightRepository,
        IUserRepository userRepository,
        IAuditLogRepository auditLogRepository)
    {
        _flightRepository = flightRepository;
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
    }

    /// <summary>
    /// Runs the admin dashboard menu loop.
    /// </summary>
    /// <param name="user">The authenticated admin user.</param>
    public async Task RunAsync(User user)
    {
        var running = true;
        while (running)
        {
            var choice = ConsoleHelper.DisplayMenu("Admin Dashboard", new[]
            {
                "Manage Flights",
                "System Oversight",
                "Staff Management",
                "Logout"
            });

            switch (choice)
            {
                case 1:
                    // TODO: Implement flight creation flow
                    ConsoleHelper.Info("Manage Flights - coming soon.");
                    ConsoleHelper.PressEnterToContinue();
                    break;
                case 2:
                    // TODO: Implement system oversight view
                    ConsoleHelper.Info("System Oversight - coming soon.");
                    ConsoleHelper.PressEnterToContinue();
                    break;
                case 3:
                    // TODO: Implement staff management flow
                    ConsoleHelper.Info("Staff Management - coming soon.");
                    ConsoleHelper.PressEnterToContinue();
                    break;
                case 4:
                    running = false;
                    ConsoleHelper.Info("Logging out...");
                    break;
            }
        }
    }
}
