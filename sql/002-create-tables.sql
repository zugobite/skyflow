-- ============================================
-- SkyFlow Terminal Manager
-- 002-create-tables.sql
-- Creates all tables and relationships
-- ============================================

USE SkyFlowDB;
GO

-- ============================================
-- Users table (base for Admin and GateAgent)
-- ============================================
CREATE TABLE Users (
    UserId          INT IDENTITY(1,1) PRIMARY KEY,
    Username        NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(200) NOT NULL,
    Role            NVARCHAR(20)  NOT NULL CHECK (Role IN ('Admin', 'GateAgent')),
    FullName        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(100) NOT NULL
);
GO

-- ============================================
-- Airports table (lookup)
-- ============================================
CREATE TABLE Airports (
    AirportId       INT IDENTITY(1,1) PRIMARY KEY,
    Code            NVARCHAR(10)  NOT NULL UNIQUE,
    Name            NVARCHAR(150) NOT NULL,
    City            NVARCHAR(100) NOT NULL,
    Country         NVARCHAR(100) NOT NULL
);
GO

-- ============================================
-- Aircraft table
-- ============================================
CREATE TABLE Aircraft (
    AircraftId      INT IDENTITY(1,1) PRIMARY KEY,
    RegistrationNo  NVARCHAR(20)  NOT NULL UNIQUE,
    Model           NVARCHAR(100) NOT NULL,
    Capacity        INT           NOT NULL CHECK (Capacity > 0)
);
GO

-- ============================================
-- Flights table (Many-to-One → Airport, FK → User)
-- ============================================
CREATE TABLE Flights (
    FlightId            INT IDENTITY(1,1) PRIMARY KEY,
    FlightNumber        NVARCHAR(20)  NOT NULL UNIQUE,
    OriginAirportId     INT           NOT NULL,
    DestinationAirportId INT          NOT NULL,
    DepartureTime       DATETIME      NOT NULL,
    Status              NVARCHAR(20)  NOT NULL DEFAULT 'Scheduled'
                        CHECK (Status IN ('Scheduled', 'Boarding', 'Departed', 'Cancelled')),
    GateAgentId         INT           NULL,

    CONSTRAINT FK_Flights_OriginAirport     FOREIGN KEY (OriginAirportId)      REFERENCES Airports(AirportId),
    CONSTRAINT FK_Flights_DestinationAirport FOREIGN KEY (DestinationAirportId) REFERENCES Airports(AirportId),
    CONSTRAINT FK_Flights_GateAgent          FOREIGN KEY (GateAgentId)          REFERENCES Users(UserId)
);
GO

-- ============================================
-- FlightAssignments table (One-to-One with Flight)
-- ============================================
CREATE TABLE FlightAssignments (
    FlightAssignmentId  INT IDENTITY(1,1) PRIMARY KEY,
    FlightId            INT NOT NULL UNIQUE,
    AircraftId          INT NOT NULL,

    CONSTRAINT FK_FlightAssignments_Flight   FOREIGN KEY (FlightId)   REFERENCES Flights(FlightId),
    CONSTRAINT FK_FlightAssignments_Aircraft FOREIGN KEY (AircraftId) REFERENCES Aircraft(AircraftId)
);
GO

-- ============================================
-- Passengers table (One-to-Many from User)
-- ============================================
CREATE TABLE Passengers (
    PassengerId     INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(100) NOT NULL,
    PassportNumber  NVARCHAR(50)  NOT NULL,
    NationalId      NVARCHAR(50)  NOT NULL,
    CreatedByUserId INT           NOT NULL,

    CONSTRAINT FK_Passengers_CreatedByUser FOREIGN KEY (CreatedByUserId) REFERENCES Users(UserId)
);
GO

-- ============================================
-- Bookings table (One-to-Many from Passenger & Flight)
-- ============================================
CREATE TABLE Bookings (
    BookingId       INT IDENTITY(1,1) PRIMARY KEY,
    PassengerId     INT           NOT NULL,
    FlightId        INT           NOT NULL,
    Status          NVARCHAR(20)  NOT NULL DEFAULT 'Booked'
                    CHECK (Status IN ('Booked', 'CheckedIn', 'Boarded')),
    SeatNumber      NVARCHAR(10)  NULL,

    CONSTRAINT FK_Bookings_Passenger FOREIGN KEY (PassengerId) REFERENCES Passengers(PassengerId),
    CONSTRAINT FK_Bookings_Flight    FOREIGN KEY (FlightId)    REFERENCES Flights(FlightId)
);
GO

-- ============================================
-- Baggage table (One-to-Many from Booking)
-- ============================================
CREATE TABLE Baggage (
    BaggageId       INT IDENTITY(1,1) PRIMARY KEY,
    BookingId       INT            NOT NULL,
    Weight          DECIMAL(5,2)   NOT NULL CHECK (Weight > 0),
    TagNumber       NVARCHAR(20)   NOT NULL UNIQUE,

    CONSTRAINT FK_Baggage_Booking FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId)
);
GO

-- ============================================
-- Crew table (One-to-Many from User)
-- ============================================
CREATE TABLE Crew (
    CrewId          INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(100) NOT NULL,
    Role            NVARCHAR(50)  NOT NULL,
    AssignedByUserId INT          NOT NULL,

    CONSTRAINT FK_Crew_AssignedByUser FOREIGN KEY (AssignedByUserId) REFERENCES Users(UserId)
);
GO

-- ============================================
-- FlightLogs table (One-to-Many from Flight)
-- ============================================
CREATE TABLE FlightLogs (
    LogId           INT IDENTITY(1,1) PRIMARY KEY,
    FlightId        INT           NOT NULL,
    Action          NVARCHAR(100) NOT NULL,
    Timestamp       DATETIME      NOT NULL DEFAULT GETUTCDATE(),
    Details         NVARCHAR(500) NULL,

    CONSTRAINT FK_FlightLogs_Flight FOREIGN KEY (FlightId) REFERENCES Flights(FlightId)
);
GO

-- ============================================
-- Notifications table (One-to-Many from User)
-- ============================================
CREATE TABLE Notifications (
    NotificationId  INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT           NOT NULL,
    Message         NVARCHAR(500) NOT NULL,
    IsRead          BIT           NOT NULL DEFAULT 0,
    CreatedAt       DATETIME      NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Notifications_User FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- ============================================
-- AuditLogs table (One-to-Many from User)
-- ============================================
CREATE TABLE AuditLogs (
    AuditLogId      INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT           NOT NULL,
    Action          NVARCHAR(100) NOT NULL,
    EntityType      NVARCHAR(50)  NOT NULL,
    EntityId        INT           NOT NULL,
    Timestamp       DATETIME      NOT NULL DEFAULT GETUTCDATE(),
    Details         NVARCHAR(500) NULL,

    CONSTRAINT FK_AuditLogs_User FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

PRINT 'All tables created successfully.';
GO
