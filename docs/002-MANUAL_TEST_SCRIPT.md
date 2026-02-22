# SkyFlow Manual Test Script

This document provides a step-by-step walkthrough to verify the core functionality of the SkyFlow application, covering both the Admin and Gate Agent workflows.

## Prerequisites

1. Ensure the SQL Server database is running and seeded.
   - **Recommended:** Run `docker compose up -d` in the root directory.
   - **Manual:** Ensure SQL Server is running and seed scripts in `sql/` have been executed.
2. Ensure the application is built and running (`dotnet run --project src/SkyFlow.Console`).

---

## Test Scenario 1: Admin Workflow

**Objective:** Verify that an Admin can log in, create a flight, view the system oversight, add a new staff member, and log out successfully.

### Step 1: Admin Login

1. At the welcome screen, enter the Admin username: `admin`
2. Enter the Admin password: `Password123!`
3. **Expected Result:** Successful login. The system displays the "Admin Dashboard" with options for Manage Flights, System Oversight, Staff Management, View Audit Logs, and Logout.

### Step 2: Create a Flight

1. Select option `1` (Manage Flights).
2. Select option `1` (Create Flight).
3. Enter a Flight Number (e.g., `SF999`).
4. Enter Origin (e.g., `JFK`).
5. Enter Destination (e.g., `LHR`).
6. Enter Departure Time (e.g., `2026-12-01 14:00`).
7. Enter Arrival Time (e.g., `2026-12-01 22:00`).
8. Enter Aircraft Type (e.g., `Boeing 777`).
9. Enter Capacity (e.g., `300`).
10. **Expected Result:** The system displays a success message indicating the flight was created.

### Step 3: View System Oversight

1. From the Admin Dashboard, select option `2` (System Oversight).
2. **Expected Result:** A formatted ASCII table is displayed showing all flights, including the newly created `SF999`. The table should display Flight Number, Route, Times, Status, and Occupancy.

### Step 4: Add Staff (Gate Agent)

1. From the Admin Dashboard, select option `3` (Staff Management).
2. Select option `1` (Add Gate Agent).
3. Enter a new Username (e.g., `agent_smith`).
4. Enter a Password (e.g., `SecurePass1!`).
5. Enter Full Name (e.g., `John Smith`).
6. **Expected Result:** The system displays a success message indicating the new Gate Agent account was created.

### Step 5: Logout

1. From the Admin Dashboard, select option `5` (Logout).
2. **Expected Result:** The system returns to the main Login screen.

---

## Test Scenario 2: Gate Agent Workflow

**Objective:** Verify that a Gate Agent can log in, view a flight manifest, check in a passenger (including baggage), board the flight, and log out.

### Step 1: Gate Agent Login

1. At the welcome screen, enter the newly created Gate Agent username: `agent_smith` (or use the seeded `gate.agent1`).
2. Enter the password: `SecurePass1!` (or `Password123!` for `gate.agent1`).
3. **Expected Result:** Successful login. The system displays the "Gate Agent Dashboard" with options for Flight Manifest, Passenger Check-in, Boarding Gate, View Notifications, and Logout.

### Step 2: View Flight Manifest

1. Select option `1` (Flight Manifest).
2. Enter a valid Flight ID (e.g., `1` for the seeded flight).
3. **Expected Result:** A formatted ASCII table is displayed showing all passengers booked on that flight, along with their current status (Booked, CheckedIn, Boarded).

### Step 3: Check In Passenger & Add Baggage

1. From the Gate Agent Dashboard, select option `2` (Passenger Check-in).
2. Enter the Passenger ID of a passenger currently in "Booked" status (e.g., `1`).
3. The system will prompt: "Add baggage for this passenger? (y/n)". Enter `y`.
4. Enter the baggage weight in kg (e.g., `23.5`).
5. **Expected Result:** The system generates a baggage tag, saves the baggage record, updates the passenger's status to `CheckedIn`, and displays a success message.

### Step 4: Board Flight

1. From the Gate Agent Dashboard, select option `3` (Boarding Gate).
2. Enter the Flight ID (e.g., `1`).
3. The system displays the current status (e.g., `Scheduled`).
4. Enter the new status: `Boarding`.
5. **Expected Result:** The system successfully transitions the flight status to `Boarding` and logs the transition. (Note: Attempting an invalid transition, like `Departed` to `Boarding`, should result in an error message).

### Step 5: Logout

1. From the Gate Agent Dashboard, select option `5` (Logout).
2. **Expected Result:** The system returns to the main Login screen.

---

## Verification Checklist

- [ ] Database seeded successfully without errors.
- [ ] Application builds with zero warnings (`dotnet build`).
- [ ] Admin can create entities and view oversight tables.
- [ ] Gate Agent can mutate passenger and flight states.
- [ ] Passwords are not displayed in plain text during login.
- [ ] ASCII tables render correctly without breaking alignment.
- [ ] Application does not crash on invalid input (e.g., entering letters for a Flight ID).
