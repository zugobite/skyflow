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
                    await FlightManifestAsync();
                    break;
                case 2:
                    await PassengerCheckInAsync(user);
                    break;
                case 3:
                    await BoardingGateAsync(user);
                    break;
                case 4:
                    running = false;
                    ConsoleHelper.Info("Logging out...");
                    break;
            }
        }
    }

    private async Task FlightManifestAsync()
    {
        ConsoleHelper.DisplayDivider("Flight Manifest");

        var flightIdStr = ConsoleHelper.Prompt("Enter Flight ID");
        if (!int.TryParse(flightIdStr, out int flightId)) return;

        try
        {
            var manifest = await _bookingRepository.GetManifestAsync(flightId);
            
            var headers = new[] { "Booking ID", "Passenger Name", "Passport", "Seat", "Status" };
            var rows = new List<string[]>();

            foreach (var record in manifest)
            {
                rows.Add(new string[]
                {
                    record.BookingId.ToString(),
                    (string)record.FullName,
                    (string)record.PassportNumber,
                    (string)(record.SeatNumber ?? "Unassigned"),
                    (string)record.Status
                });
            }

            if (rows.Count == 0)
            {
                ConsoleHelper.Info("No passengers found for this flight.");
            }
            else
            {
                ConsoleTableEngine.Render(headers, rows);
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.Error($"Failed to load manifest: {ex.Message}");
        }

        ConsoleHelper.PressEnterToContinue();
    }

    private async Task PassengerCheckInAsync(User agent)
    {
        ConsoleHelper.DisplayDivider("Passenger Check-in");

        var flightIdStr = ConsoleHelper.Prompt("Enter Flight ID");
        if (!int.TryParse(flightIdStr, out int flightId)) return;

        var searchTerm = ConsoleHelper.Prompt("Enter Passenger Passport or ID");
        if (string.IsNullOrWhiteSpace(searchTerm)) return;

        try
        {
            var passengers = await _passengerRepository.SearchByIdOrPassportAsync(searchTerm);
            var passenger = passengers.FirstOrDefault();

            if (passenger == null)
            {
                ConsoleHelper.Error("Passenger not found.");
                ConsoleHelper.PressEnterToContinue();
                return;
            }

            var bookings = await _bookingRepository.GetByPassengerAsync(passenger.PassengerId);
            var booking = bookings.FirstOrDefault(b => b.FlightId == flightId);

            if (booking == null)
            {
                ConsoleHelper.Error("Passenger is not booked on this flight.");
                ConsoleHelper.PressEnterToContinue();
                return;
            }

            if (booking.Status == SkyFlow.Core.Enums.BookingStatus.CheckedIn || 
                booking.Status == SkyFlow.Core.Enums.BookingStatus.Boarded)
            {
                ConsoleHelper.Info($"Passenger is already {booking.Status}.");
                ConsoleHelper.PressEnterToContinue();
                return;
            }

            var success = await _bookingRepository.UpdateStatusAsync(booking.BookingId, SkyFlow.Core.Enums.BookingStatus.CheckedIn.ToString());

            if (success)
            {
                await _auditLogRepository.CreateAsync(new AuditLog
                {
                    UserId = agent.UserId,
                    Action = "CheckIn",
                    EntityType = "Booking",
                    EntityId = booking.BookingId,
                    Details = $"Checked in passenger {passenger.FullName} for flight {flightId}"
                });

                ConsoleHelper.Success($"Passenger {passenger.FullName} successfully checked in.");
            }
            else
            {
                ConsoleHelper.Error("Failed to update booking status.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.Error($"An error occurred: {ex.Message}");
        }

        ConsoleHelper.PressEnterToContinue();
    }

    private async Task BoardingGateAsync(User agent)
    {
        ConsoleHelper.DisplayDivider("Boarding Gate Management");

        var flightIdStr = ConsoleHelper.Prompt("Enter Flight ID");
        if (!int.TryParse(flightIdStr, out int flightId)) return;

        var flight = await _flightRepository.GetByIdAsync(flightId);
        if (flight == null)
        {
            ConsoleHelper.Error("Flight not found.");
            ConsoleHelper.PressEnterToContinue();
            return;
        }

        ConsoleHelper.Info($"Current Flight Status: {flight.Status}");
        var newStatusStr = ConsoleHelper.Prompt("Enter new status (Boarding/Departed/Cancelled)");
        
        if (!Enum.TryParse<SkyFlow.Core.Enums.FlightStatus>(newStatusStr, true, out var newStatus))
        {
            ConsoleHelper.Error("Invalid status.");
            ConsoleHelper.PressEnterToContinue();
            return;
        }

        try
        {
            var success = await _flightRepository.UpdateStatusAsync(flightId, newStatus.ToString());

            if (success)
            {
                await _flightLogRepository.CreateAsync(new FlightLog
                {
                    FlightId = flightId,
                    Action = "StatusChanged",
                    Details = $"Status changed from {flight.Status} to {newStatus}"
                });

                await _auditLogRepository.CreateAsync(new AuditLog
                {
                    UserId = agent.UserId,
                    Action = "UpdateFlightStatus",
                    EntityType = "Flight",
                    EntityId = flightId,
                    Details = $"Updated flight {flightId} status to {newStatus}"
                });

                ConsoleHelper.Success($"Flight status updated to {newStatus}.");
            }
            else
            {
                ConsoleHelper.Error("Failed to update flight status.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.Error($"An error occurred: {ex.Message}");
        }

        ConsoleHelper.PressEnterToContinue();
    }
}
