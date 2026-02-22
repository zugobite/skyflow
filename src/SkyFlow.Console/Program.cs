using Microsoft.Extensions.Configuration;
using SkyFlow.Data;
using SkyFlow.Data.Repositories;
using SkyFlow.Console.Controllers;
using SkyFlow.Console.Helpers;

namespace SkyFlow.Console;

/// <summary>
/// Application entry point for the SkyFlow Terminal Manager.
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("SkyFlowDB")
            ?? throw new InvalidOperationException("Connection string 'SkyFlowDB' not found in appsettings.json.");

        // Initialise data access
        var context = new DapperContext(connectionString);
        var userRepository = new UserRepository(context);
        var flightRepository = new FlightRepository(context);
        var passengerRepository = new PassengerRepository(context);
        var bookingRepository = new BookingRepository(context);
        var baggageRepository = new BaggageRepository(context);
        var auditLogRepository = new AuditLogRepository(context);
        var flightLogRepository = new FlightLogRepository(context);
        var notificationRepository = new NotificationRepository(context);

        // Display welcome banner
        ConsoleHelper.DisplayBanner();

        // Main application loop
        var running = true;
        while (running)
        {
            var authController = new AuthController(userRepository, auditLogRepository);
            var user = await authController.LoginAsync();

            if (user == null)
            {
                running = false;
                break;
            }

            // Dispatch to role-specific dashboard
            switch (user)
            {
                case SkyFlow.Core.Models.Admin:
                    var adminDashboard = new AdminDashboard(
                        flightRepository, userRepository, auditLogRepository, notificationRepository);
                    await adminDashboard.RunAsync(user);
                    break;

                case SkyFlow.Core.Models.GateAgent:
                    var gateAgentDashboard = new GateAgentDashboard(
                        flightRepository, passengerRepository, bookingRepository,
                        baggageRepository, flightLogRepository, notificationRepository,
                        auditLogRepository);
                    await gateAgentDashboard.RunAsync(user);
                    break;
            }
        }

        ConsoleHelper.DisplayFarewell();
    }
}
