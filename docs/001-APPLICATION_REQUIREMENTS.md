# PROGRAMMING 741 ASSIGNMENT

## SkyFlow Terminal Manager - Console Application

---

## Project Overview

You are tasked with developing a **Console-Based Airport & Airline
Management System** called **SkyFlow**.
The goal of this project is to apply advanced **Object-Oriented
Programming (OOP)** principles while managing persistent data in a **SQL
Server Database**.

This project requires you to build the core engine from scratch and
demonstrate: - Object interaction - Inheritance and polymorphism -
Database integration - Structured data visualization in a console
environment

---

# Project Requirements

## 1. Core OOP Implementation

Your solution must explicitly demonstrate the four pillars of OOP:

### Inheritance

- Create a base `User` class.
- Implement derived classes:
  - `Admin`
  - `GateAgent`

### Encapsulation

- Use private fields and public properties/methods.
- Example: Flight status must only be changed via a method such as
  `DepartFlight()`.

### Polymorphism

- Use abstract or virtual methods.
- `DisplayDashboard()` must show different menus based on the
  logged-in user role.

### Abstraction

- Use Interfaces (e.g., `IDataRepository`) to separate database logic
  from business logic.

---

## 2. Database Integration

The system must connect to a **SQL Server Database**.

You are required to:

- Use **ADO.NET (SqlConnection, SqlCommand)** or **Dapper** for CRUD
  operations.
- Ensure all data persists after application restart.
- Seed initial data via SQL scripts.
- _Optional:_ Use Docker containers for database portability.

---

## 3. Data Visualization (Console Table Engine)

Since this is a console application, readability is critical.

You must implement a helper class or method that renders SQL data in a
tabular format:

- Columns must be aligned.
- Headers must be clear.
- Use ASCII characters (`|`, `-`, `+`) to create borders.

---

# User Workflow

## Phase 1: Authentication

- Application starts with a Login screen.
- Validate credentials against the `Users` table.
- Redirect user to dashboard based on role.

---

## Phase 2: Role-Specific Actions

### Admin Workflow

1.  **Manage Flights**
    - Create flight schedules (Flight Number, Origin, Destination,
      Departure Time, Aircraft Capacity).
2.  **System Oversight**
    - View master table of flights and current occupancy.
3.  **Staff Management**
    - Add new staff members.

---

### Gate Agent Workflow

1.  **Flight Manifest**
    - Select flight and view all registered passengers.
2.  **Passenger Check-in**
    - Search by ID or Passport.
    - Update status to `CheckedIn`.
3.  **Boarding Gate**
    - Finalize flight.
    - Update status to `Boarded` or `Departed`.

---

# Entity Relationships

Your design should reflect the following relationships:

### One-to-Many

- User → Passenger
- User → Crew
- Passenger → Booking
- Flight → Booking
- Flight → FlightLog
- User → Notification
- Booking → Baggage
- Aircraft → FlightAssignment
- User → AuditLog

### One-to-One

- Flight → FlightAssignment

### Many-to-One

- Flight → Airport

### Foreign Key

- Flight.GateAgentId → User.UserId

---

# Submission Instructions

- Submit a single zipped folder containing complete project files.
- Maximum file size: **50MB**.
- Ensure the application is fully functional before submission.
- Include:
  - PDF report
  - Source code
  - SQL seed scripts
  - Supporting files
- Non-functional applications receive **zero marks**.
