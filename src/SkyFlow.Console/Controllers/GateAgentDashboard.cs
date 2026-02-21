using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Models;
using SkyFlow.Console.Helpers;

namespace SkyFlow.Console.Controllers;

/// <summary>
/// Gate agent-specific dashboard handling flight manifests, check-in, and boarding.
/// </summary>
public class GateAgentDashboard
{
    private readonly IFlightRepository _flightRepository;
    private readonly IPassengerRepository _passengerRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IBaggageRepository _baggageRepository;
    private readonly IFlightLogRepository _flightLogRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GateAgentDashboard"/> class.
    /// </summary>
    public GateAgentDashboard(
        IFlightRepository flightRepository,
        IPassengerRepository passengerRepository,
        IBookingRepository bookingRepository,
        IBaggageRepository baggageRepository,
        IFlightLogRepository flightLogRepository,
        INotificationRepository notificationRepository,
        IAuditLogRepository auditLogRepository)
    {
        _flightRepository = flightRepository;
        _passengerRepository = passengerRepository;
        _bookingRepository = bookingRepository;
        _baggageRepository = baggageRepository;
        _flightLogRepository = flightLogRepository;
        _notificationRepository = notificationRepository;
        _auditLogRepository = auditLogRepository;
    }

    /// <summary>
    /// Runs the gate agent dashboard menu loop.
    /// </summary>
    /// <param name="user">The authenticated gate agent user.</param>
    public async Task RunAsync(User user)
    {
        var running = true;
        while (running)
        {
            var choice = ConsoleHelper.DisplayMenu("Gate Agent Dashboard", new[]
            {
                "Flight Manifest",
                "Passenger Check-in",
                "Boarding Gate",
                "Logout"
            });

            switch (choice)
            {
                case 1:
                    // TODO: Implement flight manifest view
                    ConsoleHelper.Info("Flight Manifest - coming soon.");
                    ConsoleHelper.PressEnterToContinue();
                    break;
                case 2:
                    // TODO: Implement passenger check-in flow
                    ConsoleHelper.Info("Passenger Check-in - coming soon.");
                    ConsoleHelper.PressEnterToContinue();
                    break;
                case 3:
                    // TODO: Implement boarding gate flow
                    ConsoleHelper.Info("Boarding Gate - coming soon.");
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
