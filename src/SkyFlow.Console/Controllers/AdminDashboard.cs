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
                    await ManageFlightsAsync(user);
                    break;
                case 2:
                    await SystemOversightAsync();
                    break;
                case 3:
                    await StaffManagementAsync(user);
                    break;
                case 4:
                    running = false;
                    ConsoleHelper.Info("Logging out...");
                    break;
            }
        }
    }

    private async Task ManageFlightsAsync(User admin)
    {
        ConsoleHelper.DisplayDivider("Manage Flights - Create Schedule");

        var flightNumber = ConsoleHelper.Prompt("Flight Number (e.g., SF606)");
        if (string.IsNullOrWhiteSpace(flightNumber)) return;

        var originIdStr = ConsoleHelper.Prompt("Origin Airport ID (1-5)");
        if (!int.TryParse(originIdStr, out int originId)) return;

        var destIdStr = ConsoleHelper.Prompt("Destination Airport ID (1-5)");
        if (!int.TryParse(destIdStr, out int destId)) return;

        var depTimeStr = ConsoleHelper.Prompt("Departure Time (yyyy-MM-dd HH:mm)");
        if (!DateTime.TryParse(depTimeStr, out DateTime depTime))
        {
            ConsoleHelper.Error("Invalid date format.");
            ConsoleHelper.PressEnterToContinue();
            return;
        }

        var regNo = ConsoleHelper.Prompt("Aircraft Registration No (e.g., ZS-SFD)");
        var model = ConsoleHelper.Prompt("Aircraft Model (e.g., Boeing 737)");
        var capacityStr = ConsoleHelper.Prompt("Aircraft Capacity");
        if (!int.TryParse(capacityStr, out int capacity)) return;

        var flight = new Flight
        {
            FlightNumber = flightNumber,
            OriginAirportId = originId,
            DestinationAirportId = destId,
            DepartureTime = depTime,
            Status = SkyFlow.Core.Enums.FlightStatus.Scheduled,
            GateAgentId = admin.UserId // Assigning admin as default gate agent for now
        };

        var aircraft = new Aircraft
        {
            RegistrationNo = regNo,
            Model = model,
            Capacity = capacity
        };

        try
        {
            var flightId = await _flightRepository.CreateFlightWithAircraftAsync(flight, aircraft);
            
            await _auditLogRepository.CreateAsync(new AuditLog
            {
                UserId = admin.UserId,
                Action = "CreateFlight",
                EntityType = "Flight",
                EntityId = flightId,
                Details = $"Created flight {flightNumber} with aircraft {regNo}"
            });

            ConsoleHelper.Success($"Flight {flightNumber} created successfully with ID {flightId}.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.Error($"Failed to create flight: {ex.Message}");
        }

        ConsoleHelper.PressEnterToContinue();
    }

    private async Task SystemOversightAsync()
    {
        ConsoleHelper.DisplayDivider("System Oversight - Master Flight Table");

        try
        {
            var flights = await _flightRepository.GetFlightsWithOccupancyAsync();
            
            var headers = new[] { "ID", "Flight No", "Route", "Departure", "Status", "Bookings", "Capacity", "Occupancy %" };
            var rows = new List<string[]>();

            foreach (var f in flights)
            {
                int bookings = f.BookingCount ?? 0;
                int capacity = f.Capacity ?? 0;
                double occupancy = capacity > 0 ? (double)bookings / capacity * 100 : 0;

                rows.Add(new string[]
                {
                    f.FlightId.ToString(),
                    (string)f.FlightNumber,
                    $"{f.Origin} -> {f.Destination}",
                    ((DateTime)f.DepartureTime).ToString("yyyy-MM-dd HH:mm"),
                    (string)f.Status,
                    bookings.ToString(),
                    capacity.ToString(),
                    $"{occupancy:F1}%"
                });
            }

            ConsoleTableEngine.Render(headers, rows);
        }
        catch (Exception ex)
        {
            ConsoleHelper.Error($"Failed to load flights: {ex.Message}");
        }

        ConsoleHelper.PressEnterToContinue();
    }

    private async Task StaffManagementAsync(User admin)
    {
        ConsoleHelper.DisplayDivider("Staff Management - Add New Staff");

        var username = ConsoleHelper.Prompt("Username");
        if (string.IsNullOrWhiteSpace(username)) return;

        var password = ConsoleHelper.PromptPassword("Password");
        if (string.IsNullOrWhiteSpace(password)) return;

        var role = ConsoleHelper.Prompt("Role (Admin/GateAgent)");
        if (role != "Admin" && role != "GateAgent")
        {
            ConsoleHelper.Error("Invalid role. Must be 'Admin' or 'GateAgent'.");
            ConsoleHelper.PressEnterToContinue();
            return;
        }

        var fullName = ConsoleHelper.Prompt("Full Name");
        var email = ConsoleHelper.Prompt("Email");

        User newUser;
        if (role == "Admin")
        {
            newUser = new Admin
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = SkyFlow.Core.Enums.UserRole.Admin,
                FullName = fullName,
                Email = email
            };
        }
        else
        {
            newUser = new GateAgent
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = SkyFlow.Core.Enums.UserRole.GateAgent,
                FullName = fullName,
                Email = email
            };
        }

        try
        {
            var userId = await _userRepository.CreateAsync(newUser);

            await _auditLogRepository.CreateAsync(new AuditLog
            {
                UserId = admin.UserId,
                Action = "CreateUser",
                EntityType = "User",
                EntityId = userId,
                Details = $"Created new {role} user: {username}"
            });

            ConsoleHelper.Success($"User {username} created successfully with ID {userId}.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.Error($"Failed to create user: {ex.Message}");
        }

        ConsoleHelper.PressEnterToContinue();
    }
}
