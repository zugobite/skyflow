# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added

- Nothing yet

### Changed

- Nothing yet

### Fixed

- Nothing yet

---

## [0.4.0] - 2026-02-22

### Added

- **Docker Support:**
  - Added `docker-compose.yml` for easy database setup.
  - Added automated `sql-setup` container to initialize the database and seed data automatically.

### Fixed

- **Cross-Platform Compatibility:**
  - Updated `docker-compose.yml` with `platform: linux/amd64` to support Apple Silicon (M1/M2/M3) chips.
- **Authentication:**
  - Regenerated BCrypt password hashes in `003-seed-data.sql` to fix login failures.
  - Default password `Password123!` now works correctly for all seeded users.

## [0.3.0] - 2026-02-22

### Added

- **Baggage Handling:**
  - Added baggage prompt during passenger check-in in `GateAgentDashboard.cs`.
  - Implemented `IBaggageRepository` to save baggage weight and generate a unique tag number.
- **Notifications:**
  - Added `View Notifications` menu option to `GateAgentDashboard.cs`.
  - Implemented `INotificationRepository` to fetch unread notifications and mark them as read.
  - Added logic to `AdminDashboard.cs` to automatically create a notification for the assigned Gate Agent when a new flight is created.
- **Audit & Flight Logs:**
  - Added `View Audit Logs` menu option to `AdminDashboard.cs`.
  - Implemented `GetAllAsync` in `IAuditLogRepository` to view system-wide audit logs.
  - Added `FlightLog` creation when a flight status changes in `GateAgentDashboard.cs`.
- **Strict OOP Encapsulation:**
  - Refactored `GateAgentDashboard.cs` to use `Flight.BeginBoarding()`, `Flight.DepartFlight()`, and `Flight.CancelFlight()` methods to enforce valid state transitions before saving to the database.

### Changed

- Updated `ConsoleHelper.cs` banner to display `v1.0.0`.

### Fixed

- Fixed `IAuditLogRepository` missing `GetAllAsync` method.

---

## [0.2.0] - 2026-02-22

### Added

- **Admin Dashboard Features:**
  - Implemented `ManageFlightsAsync` to create new flights and assign aircraft transactionally.
  - Implemented `SystemOversightAsync` to view a master table of all flights with calculated occupancy percentages.
  - Implemented `StaffManagementAsync` to create new `Admin` or `GateAgent` users with BCrypt password hashing.
- **Gate Agent Dashboard Features:**
  - Implemented `FlightManifestAsync` to view a formatted table of all passengers booked on a specific flight.
  - Implemented `PassengerCheckInAsync` to search for passengers by ID/Passport and update their booking status to `CheckedIn`.
  - Implemented `BoardingGateAsync` to finalize flights by updating their status to `Boarding`, `Departed`, or `Cancelled`.
- **Data Access Layer:**
  - Added `CreateFlightWithAircraftAsync` to `IFlightRepository` and `FlightRepository` for complex transactional inserts.
  - Added `GetManifestAsync` to `IBookingRepository` and `BookingRepository` to join `Bookings` and `Passengers` for the manifest view.

### Changed

- Updated `Program.cs` to use `AppContext.BaseDirectory` instead of `Directory.GetCurrentDirectory()` to ensure `appsettings.json` is found reliably when running the compiled executable.
- Updated `ConsoleHelper.cs` to display the SkyFlow logo on a single line for a cleaner terminal appearance.

### Fixed

- Fixed type-casting issues in `AdminDashboard.cs` and `GateAgentDashboard.cs` when passing dynamic Dapper results to the `ConsoleTableEngine`.

---

## [0.1.0] - 2026-02-21

### Added

#### Project Structure

- Multi-project .NET 8 solution with `SkyFlow.Core`, `SkyFlow.Data`, and `SkyFlow.Console`
- `SkyFlow.Core` - Domain models, enums, interfaces, and business logic (no DB dependency)
- `SkyFlow.Data` - Dapper-based repository implementations with `DapperContext` connection factory
- `SkyFlow.Console` - Entry point, console rendering, authentication flow, and menu controllers
- `sql/` directory for database creation, table definitions, and seed data scripts
- `docs/` directory for project documentation

#### Configuration & Dependencies

- Dapper and Microsoft.Data.SqlClient for SQL Server data access
- BCrypt.Net-Next for secure password hashing
- Microsoft.Extensions.Configuration for `appsettings.json` connection string management
- `appsettings.json` with SQL Server connection string placeholder

#### Documentation & GitHub Meta

- `README.md` with badges, features, quick start, project structure, and architecture overview
- `CHANGELOG.md` following Keep a Changelog format
- `CONTRIBUTING.md` with development setup, style guidelines, and PR process
- `LICENSE` - MIT License
- `.github/SECURITY.md` with vulnerability reporting policy
- `.github/ISSUE_TEMPLATE/bug_report.md` and `feature_request.md`
- `.github/PULL_REQUEST_TEMPLATE.md`
- `.github/CODEOWNERS` assigning repository ownership
- `.github/FUNDING.yml` for sponsorship configuration
- `.gitignore` for .NET, IDE, and OS files

---

[Unreleased]: https://github.com/zugobite/skyflow/compare/v0.2.0...HEAD
[0.2.0]: https://github.com/zugobite/skyflow/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/zugobite/skyflow/releases/tag/v0.1.0
