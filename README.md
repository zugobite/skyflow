# SkyFlow

A modern, console-based airport and airline management system built with C# and .NET 8, demonstrating OOP principles, SQL Server integration, and structured data visualization.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Features

- **Role-Based Authentication** - Admin and GateAgent roles with BCrypt-hashed credential verification
- **Flight Management** - Create flight schedules, assign aircraft, and track real-time status
- **Passenger Check-in** - Search passengers by ID or passport, update booking status through check-in flow
- **Boarding Gate** - Finalize flights, transition passengers from CheckedIn to Boarded/Departed
- **System Oversight** - Master flight table with live occupancy percentages and capacity tracking
- **Staff Management** - Admin-driven creation of new gate agent accounts
- **Console Table Engine** - ASCII-bordered tabular rendering for clean, readable data display
- **Audit Logging** - Every mutating action is recorded with user, timestamp, and entity details
- **SQL Server Persistence** - All data persists across application restarts via Dapper ORM
- **Modular Architecture** - Multi-project solution with clean separation of Domain, Data, and UI layers

## Table of Contents

- [Quick Start](#quick-start)
- [Documentation](#documentation)
- [Menu Commands & Actions](#menu-commands--actions)
- [Usage Examples](#usage-examples)
- [Project Structure](#project-structure)
- [Class Hierarchy](#class-hierarchy)
- [Development](#development)
- [Contributing](#contributing)
- [Disclaimer](#disclaimer)
- [Security](#security)
- [License](#license)

## Quick Start

### Prerequisites

- **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** or higher
- **[Docker Desktop](https://www.docker.com/products/docker-desktop)** (Recommended for database)
- *Alternatively:* Local SQL Server instance

### Cross-Platform Setup (Windows & macOS)

The easiest way to get started is using Docker to host the SQL Server database. This ensures compatibility across Windows and macOS (including Apple Silicon).

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/zugobite/skyflow.git
    cd skyflow
    ```

2.  **Start the Database:**
    We provide a `docker-compose.yml` that sets up SQL Server and automatically runs all seed scripts.
    ```bash
    docker compose up -d
    ```
    *Wait about 30 seconds for the database to start and the seed data to be applied.*

3.  **Run the Application:**
    ```bash
    dotnet run --project src/SkyFlow.Console
    ```

### OS-Specific Manual Setup

If you prefer not to use Docker, follow these manual steps:

#### Windows (Local SQL Server)
1.  Ensure SQL Server (Express or Developer) is running.
2.  Open a terminal in the project root.
3.  Run the SQL scripts in order using `sqlcmd`:
    ```powershell
    sqlcmd -S localhost -i sql/001-create-database.sql
    sqlcmd -S localhost -d SkyFlowDB -i sql/002-create-tables.sql
    sqlcmd -S localhost -d SkyFlowDB -i sql/003-seed-data.sql
    ```
4.  Update `src/SkyFlow.Console/appsettings.json` with your actual connection string.

#### macOS (Manual without Docker)
*Note: Microsoft SQL Server does not run natively on macOS. You must use Docker (see Cross-Platform Setup above) or connect to a remote instance.*

### Default Login Credentials

Use these credentials to log in and test the system:

**Default Password:** `Password123!`

| Role | Username | Permissions |
| :--- | :--- | :--- |
| **Admin** | `admin` | Full system access (manage staff, flight master table) |
| **Admin** | `admin2` | Full system access |
| **Gate Agent** | `gate.agent1` | Flight management, passenger check-in & boarding |
| **Gate Agent** | `gate.agent2` | Flight management, passenger check-in & boarding |

---

## Documentation

Extensive developer documentation is available in the [`docs/`](docs/) folder:

| Document                                                             | Description                                    |
| -------------------------------------------------------------------- | ---------------------------------------------- |
| [001-APPLICATION_REQUIREMENTS.md](docs/001-APPLICATION_REQUIREMENTS.md) | Full application requirements specification    |

## Menu Commands & Actions

### Login Screen

| Prompt   | Description                              |
| -------- | ---------------------------------------- |
| Username | Enter your username from the Users table |
| Password | Enter your password (masked input)       |

### Admin Actions

| Option | Action           | Description                                         |
| ------ | ---------------- | --------------------------------------------------- |
| `1`    | Manage Flights   | Create flight schedules with aircraft assignments   |
| `2`    | System Oversight | View master flight table with occupancy percentages |
| `3`    | Staff Management | Add new gate agent accounts                         |
| `4`    | View Audit Logs  | View system-wide audit logs                         |
| `5`    | Logout           | Return to login screen                              |

### Gate Agent Actions

| Option | Action             | Description                                          |
| ------ | ------------------ | ---------------------------------------------------- |
| `1`    | Flight Manifest    | Select flight and view all registered passengers     |
| `2`    | Passenger Check-in | Search by ID or passport, update status to CheckedIn |
| `3`    | Boarding Gate      | Finalize flight, update statuses to Boarded/Departed |
| `4`    | View Notifications | View unread notifications                            |
| `5`    | Logout             | Return to login screen                               |

## Usage Examples

### Complete User Flow

```bash
# 1. Build and run
dotnet build
dotnet run --project src/SkyFlow.Console

# 2. Login
# Enter your username and password

# 3. Navigate the dashboard
# Select options by entering the corresponding number

# 4. Create a flight (Admin)
# Choose option 1 and fill in flight details

# 5. Check in a passenger (Gate Agent)
# Choose option 2, search by ID or passport, update status

# 6. Board a flight (Gate Agent)
# Choose option 3, select flight, finalize departure
```

### Quick Start Commands

```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project src/SkyFlow.Console
```

## Project Structure

```
skyflow/
├── .github/
│   ├── ISSUE_TEMPLATE/                     # Issue templates
│   │   ├── bug_report.md
│   │   └── feature_request.md
│   ├── PULL_REQUEST_TEMPLATE.md            # PR template
│   ├── SECURITY.md                         # Security policy
│   ├── CODEOWNERS                          # Code ownership rules
│   └── FUNDING.yml                         # Sponsorship info
├── src/
│   ├── SkyFlow.Core/                       # Domain layer (no DB dependency)
│   │   ├── Enums/
│   │   │   ├── UserRole.cs                 # Admin / GateAgent enum
│   │   │   ├── FlightStatus.cs             # Scheduled / Boarding / Departed / Cancelled
│   │   │   └── BookingStatus.cs            # Booked / CheckedIn / Boarded
│   │   ├── Models/
│   │   │   ├── User.cs                     # Abstract base class
│   │   │   ├── Admin.cs                    # Admin user type
│   │   │   ├── GateAgent.cs                # Gate agent user type
│   │   │   ├── Flight.cs                   # Flight with status transitions
│   │   │   ├── Airport.cs                  # Airport lookup
│   │   │   ├── Aircraft.cs                 # Aircraft with capacity
│   │   │   ├── FlightAssignment.cs         # One-to-one with Flight
│   │   │   ├── Passenger.cs                # Passenger domain model
│   │   │   ├── Booking.cs                  # Booking with status
│   │   │   ├── Baggage.cs                  # Baggage per booking
│   │   │   ├── Crew.cs                     # Crew member
│   │   │   ├── FlightLog.cs                # Flight status change log
│   │   │   ├── Notification.cs             # User notification
│   │   │   └── AuditLog.cs                 # System audit trail
│   │   └── Interfaces/
│   │       ├── IUserRepository.cs          # User data contract
│   │       ├── IFlightRepository.cs        # Flight data contract
│   │       ├── IPassengerRepository.cs     # Passenger data contract
│   │       ├── IBookingRepository.cs       # Booking data contract
│   │       ├── IBaggageRepository.cs       # Baggage data contract
│   │       ├── IAuditLogRepository.cs      # Audit log data contract
│   │       ├── IFlightLogRepository.cs     # Flight log data contract
│   │       └── INotificationRepository.cs  # Notification data contract
│   ├── SkyFlow.Data/                       # Data access layer
│   │   ├── DapperContext.cs                # SQL Server connection factory
│   │   └── Repositories/
│   │       ├── UserRepository.cs           # IUserRepository implementation
│   │       ├── FlightRepository.cs         # IFlightRepository implementation
│   │       ├── PassengerRepository.cs      # IPassengerRepository implementation
│   │       ├── BookingRepository.cs        # IBookingRepository implementation
│   │       ├── BaggageRepository.cs        # IBaggageRepository implementation
│   │       ├── AuditLogRepository.cs       # IAuditLogRepository implementation
│   │       ├── FlightLogRepository.cs      # IFlightLogRepository implementation
│   │       └── NotificationRepository.cs   # INotificationRepository implementation
│   └── SkyFlow.Console/                    # Presentation layer
│       ├── Program.cs                      # Application entry point
│       ├── appsettings.json                # Connection string configuration
│       ├── Controllers/
│       │   ├── AuthController.cs           # Login and authentication flow
│       │   ├── AdminDashboard.cs           # Admin menu handler
│       │   └── GateAgentDashboard.cs       # Gate agent menu handler
│       └── Helpers/
│           ├── ConsoleTableEngine.cs       # ASCII table rendering engine
│           └── ConsoleHelper.cs            # Input prompts, colours, formatting
├── sql/                                    # Database scripts
│   ├── 001-create-database.sql             # Create SkyFlowDB
│   ├── 002-create-tables.sql               # Create all tables
│   └── 003-seed-data.sql                   # Insert sample data
├── docs/                                   # Developer documentation
│   └── 001-APPLICATION_REQUIREMENTS        # Full application requirements
├── .gitignore                              # Git ignore rules
├── CHANGELOG.md                            # Version history
├── CONTRIBUTING.md                         # Contribution guidelines
├── LICENSE                                 # MIT License
├── README.md                               # This file
└── SkyFlow.sln                             # .NET solution file
```

## Class Hierarchy

The application uses the following class structure:

| Class                | Type       | Description                                             |
| -------------------- | ---------- | ------------------------------------------------------- |
| `User`               | Abstract   | Base class with UserId, Username, Role, and permissions |
| `Admin`              | Concrete   | Admin user with flight/staff management actions         |
| `GateAgent`          | Concrete   | Gate agent with check-in and boarding actions           |
| `UserRole`           | Enum       | Admin and GateAgent role constants                      |
| `FlightStatus`       | Enum       | Scheduled, Boarding, Departed, Cancelled                |
| `BookingStatus`      | Enum       | Booked, CheckedIn, Boarded                              |
| `Flight`             | Model      | Flight with encapsulated status transitions             |
| `Airport`            | Model      | Airport with IATA code, name, city, country             |
| `Aircraft`           | Model      | Aircraft with registration, model, capacity             |
| `FlightAssignment`   | Model      | One-to-one aircraft-to-flight mapping                   |
| `Passenger`          | Model      | Passenger with name, passport, national ID              |
| `Booking`            | Model      | Booking linking passenger to flight with status         |
| `Baggage`            | Model      | Baggage item per booking with weight and tag            |
| `Crew`               | Model      | Crew member assigned by a user                          |
| `FlightLog`          | Model      | Flight status change audit record                       |
| `Notification`       | Model      | User notification with read/unread state                |
| `AuditLog`           | Model      | System-wide audit trail entry                           |
| `DapperContext`      | Data       | SQL Server connection factory for Dapper                |
| `AuthController`     | Controller | Login screen and credential verification                |
| `AdminDashboard`     | Controller | Admin menu loop and action dispatch                     |
| `GateAgentDashboard` | Controller | Gate agent menu loop and action dispatch                |
| `ConsoleTableEngine` | Utility    | ASCII-bordered table rendering for console output       |
| `ConsoleHelper`      | Utility    | Console formatting, prompts, and input masking          |
| `Program`            | Entry      | Application entry point                                 |

## Development

```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project src/SkyFlow.Console
```

### Architecture

This project follows a layered architecture with clean project separation:

- **SkyFlow.Core** defines domain entities, enums, and repository interfaces (no DB dependency)
- **SkyFlow.Data** implements repository interfaces using Dapper against SQL Server
- **SkyFlow.Console** handles user interaction, menu flow, authentication, and console rendering

### OOP & Technical Features

| Feature        | Implementation                                                   |
| -------------- | ---------------------------------------------------------------- |
| Abstraction    | `IDataRepository` interfaces separating DB from business logic   |
| Inheritance    | `Admin` and `GateAgent` extend abstract `User` base class        |
| Polymorphism   | `DisplayDashboard()` dispatches role-specific menus              |
| Encapsulation  | Private `_status` field with `DepartFlight()` transition methods |
| Dapper ORM     | Lightweight SQL mapping for all CRUD operations                  |
| BCrypt Hashing | Secure password storage and verification                         |
| SQL Server     | Persistent relational data across application restarts           |
| Audit Logging  | Every mutating operation tracked with user and timestamp         |

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feat/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: add amazing feature'`)
4. Push to the branch (`git push origin feat/amazing-feature`)
5. Open a Pull Request

### Guidelines

- Follow the existing code style
- Add XML documentation comments for new classes and methods
- Update documentation as needed
- Keep commits atomic and well-described

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

## Disclaimer

This project is created **purely for educational and portfolio demonstration purposes** to showcase C# development skills and software engineering knowledge.

**Important notices:**

- It implements **industry-standard patterns** documented in publicly available resources including:
  - Object-oriented design principles (abstraction, inheritance, polymorphism, encapsulation)
  - Repository pattern for data access separation
  - Layered architecture with clean project boundaries
- This is **not intended for production use** without proper security audits and additional hardening
- The codebase demonstrates OOP principles, database integration, and modular design

## Security

If you discover a security vulnerability, please report it privately rather than opening a public issue. See [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
