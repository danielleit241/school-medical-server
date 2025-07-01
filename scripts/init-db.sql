-- SQL Server Database Initialization Script
-- This script is executed when the SQL Server container starts for the first time

USE master;
GO

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SchoolMedicalManagement')
BEGIN
    CREATE DATABASE [SchoolMedicalManagement];
    PRINT 'Database SchoolMedicalManagement created successfully';
END
ELSE
BEGIN
    PRINT 'Database SchoolMedicalManagement already exists';
END
GO

-- Switch to the new database
USE [SchoolMedicalManagement];
GO

-- Enable snapshot isolation (recommended for EF Core)
ALTER DATABASE [SchoolMedicalManagement] SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE [SchoolMedicalManagement] SET READ_COMMITTED_SNAPSHOT ON;
GO

PRINT 'Database initialization completed';
GO