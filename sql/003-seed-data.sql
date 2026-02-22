-- ============================================
-- SkyFlow Terminal Manager
-- 003-seed-data.sql
-- Inserts sample data for development/testing
-- ============================================
-- BCrypt hashes generated for password "Password123!"
-- Use BCrypt.Net.BCrypt.HashPassword("Password123!") to regenerate if needed
-- ============================================

USE SkyFlowDB;
GO

-- ============================================
-- Users (2 Admins, 2 Gate Agents)
-- Default password for all: Password123!
-- ============================================
INSERT INTO Users (Username, PasswordHash, Role, FullName, Email) VALUES
('admin',       '$2a$11$tolN577aa70zsLff2TYAQugUwlBcM5jbFuRp5Gq.oPqZ105Kwzox6', 'Admin',     'Sarah Mitchell',   'sarah.mitchell@skyflow.co.za'),
('admin2',      '$2a$11$tolN577aa70zsLff2TYAQugUwlBcM5jbFuRp5Gq.oPqZ105Kwzox6', 'Admin',     'James van der Berg','james.vdb@skyflow.co.za'),
('gate.agent1', '$2a$11$tolN577aa70zsLff2TYAQugUwlBcM5jbFuRp5Gq.oPqZ105Kwzox6', 'GateAgent', 'Thandi Nkosi',     'thandi.nkosi@skyflow.co.za'),
('gate.agent2', '$2a$11$tolN577aa70zsLff2TYAQugUwlBcM5jbFuRp5Gq.oPqZ105Kwzox6', 'GateAgent', 'David Botha',      'david.botha@skyflow.co.za');
GO

-- ============================================
-- Airports (5 South African airports)
-- ============================================
INSERT INTO Airports (Code, Name, City, Country) VALUES
('JNB', 'OR Tambo International Airport',          'Johannesburg', 'South Africa'),
('CPT', 'Cape Town International Airport',         'Cape Town',    'South Africa'),
('DUR', 'King Shaka International Airport',        'Durban',       'South Africa'),
('PLZ', 'Chief Dawid Stuurman International Airport','Port Elizabeth','South Africa'),
('BFN', 'Bram Fischer International Airport',      'Bloemfontein', 'South Africa');
GO

-- ============================================
-- Aircraft (3 aircraft)
-- ============================================
INSERT INTO Aircraft (RegistrationNo, Model, Capacity) VALUES
('ZS-SFA', 'Boeing 737-800',   189),
('ZS-SFB', 'Airbus A320neo',   180),
('ZS-SFC', 'Embraer E190',     100);
GO

-- ============================================
-- Flights (5 flights at various statuses)
-- ============================================
INSERT INTO Flights (FlightNumber, OriginAirportId, DestinationAirportId, DepartureTime, Status, GateAgentId) VALUES
('SF101', 1, 2, '2026-03-01 06:30:00', 'Scheduled', 3),
('SF202', 2, 3, '2026-03-01 09:15:00', 'Scheduled', 3),
('SF303', 3, 1, '2026-03-01 12:00:00', 'Boarding',  4),
('SF404', 1, 4, '2026-03-01 15:45:00', 'Scheduled', 4),
('SF505', 4, 5, '2026-02-28 08:00:00', 'Departed',  3);
GO

-- ============================================
-- Flight Assignments (aircraft to flights)
-- ============================================
INSERT INTO FlightAssignments (FlightId, AircraftId) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 1),
(5, 2);
GO

-- ============================================
-- Passengers (8 sample passengers)
-- ============================================
INSERT INTO Passengers (FullName, PassportNumber, NationalId, CreatedByUserId) VALUES
('Nelson Mandela',      'PA1234567', '5007185012081', 1),
('Desmond Tutu',        'PA2345678', '3110075012082', 1),
('Charlize Theron',     'PA3456789', '7508075012083', 1),
('Trevor Noah',         'PA4567890', '8402205012084', 1),
('Siya Kolisi',         'PA5678901', '9106185012085', 1),
('Caster Semenya',      'PA6789012', '9101075012086', 1),
('Elon Musk',           'PA7890123', '7106285012087', 1),
('Shaka Zulu',          'PA8901234', '8507155012088', 1);
GO

-- ============================================
-- Bookings (sample bookings across flights)
-- ============================================
INSERT INTO Bookings (PassengerId, FlightId, Status, SeatNumber) VALUES
(1, 1, 'Booked',    '12A'),
(2, 1, 'Booked',    '12B'),
(3, 1, 'CheckedIn', '14C'),
(4, 2, 'Booked',    '3A'),
(5, 2, 'CheckedIn', '3B'),
(6, 3, 'Boarded',   '7A'),
(7, 3, 'CheckedIn', '7B'),
(8, 4, 'Booked',    '1A');
GO

-- ============================================
-- Baggage (sample items)
-- ============================================
INSERT INTO Baggage (BookingId, Weight, TagNumber) VALUES
(1, 23.50, 'SF-BAG-001'),
(1, 12.00, 'SF-BAG-002'),
(2, 20.00, 'SF-BAG-003'),
(3, 18.75, 'SF-BAG-004'),
(4, 25.00, 'SF-BAG-005'),
(6, 22.30, 'SF-BAG-006');
GO

-- ============================================
-- Crew (sample crew members)
-- ============================================
INSERT INTO Crew (FullName, Role, AssignedByUserId) VALUES
('Captain Pieter de Villiers',  'Pilot',            1),
('First Officer Amina Patel',   'Co-Pilot',         1),
('Lindiwe Mokoena',             'Cabin Crew Lead',  1),
('Stefan Joubert',              'Cabin Crew',       1),
('Nomsa Dlamini',               'Cabin Crew',       2);
GO

-- ============================================
-- Flight Logs (sample entries)
-- ============================================
INSERT INTO FlightLogs (FlightId, Action, Timestamp, Details) VALUES
(3, 'StatusChanged', '2026-03-01 11:30:00', 'Status changed from Scheduled to Boarding'),
(5, 'StatusChanged', '2026-02-28 07:45:00', 'Status changed from Scheduled to Boarding'),
(5, 'StatusChanged', '2026-02-28 08:05:00', 'Status changed from Boarding to Departed');
GO

-- ============================================
-- Notifications (sample)
-- ============================================
INSERT INTO Notifications (UserId, Message, IsRead, CreatedAt) VALUES
(3, 'You have been assigned to flight SF101 (JNB → CPT).', 0, '2026-02-27 10:00:00'),
(3, 'You have been assigned to flight SF202 (CPT → DUR).', 0, '2026-02-27 10:05:00'),
(4, 'You have been assigned to flight SF303 (DUR → JNB).', 1, '2026-02-27 10:10:00'),
(4, 'You have been assigned to flight SF404 (JNB → PLZ).', 0, '2026-02-27 10:15:00');
GO

PRINT 'Seed data inserted successfully.';
GO
