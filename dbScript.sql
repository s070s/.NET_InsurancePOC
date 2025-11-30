-- Create the database
CREATE DATABASE InsurancePOC;
GO

-- Switch to the database
USE InsurancePOC;
GO

-- Create Clients table
CREATE TABLE Clients
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(50) NULL,
    DateOfBirth DATE NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETDATE())
);
GO

-- Create Policies table
CREATE TABLE Policies
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT NOT NULL,
    PolicyNumber NVARCHAR(50) NOT NULL,
    PolicyType NVARCHAR(50) NOT NULL,
    PremiumAmount DECIMAL(18,2) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETDATE()),

    CONSTRAINT FK_Policies_Clients
        FOREIGN KEY (ClientId)
        REFERENCES Clients(Id)
        ON DELETE CASCADE
);
GO
