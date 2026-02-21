-- ============================================
-- SkyFlow Terminal Manager
-- 001-create-database.sql
-- Creates the SkyFlowDB database
-- ============================================

USE master;
GO

-- Drop database if it exists (for clean re-creation)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'SkyFlowDB')
BEGIN
    ALTER DATABASE SkyFlowDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SkyFlowDB;
END
GO

-- Create the database
CREATE DATABASE SkyFlowDB;
GO

PRINT 'SkyFlowDB database created successfully.';
GO
