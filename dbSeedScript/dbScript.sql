-- =============================================
-- Insurance POC Database Script with Edge Case Constraints
-- =============================================
-- Create the database
CREATE DATABASE InsurancePOC;
GO

-- Switch to the database
USE InsurancePOC;
GO

-- =============================================
-- Create Clients table with constraints
-- =============================================
CREATE TABLE Clients
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(50) NULL,
    DateOfBirth DATE NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    
    -- Edge case constraints
    CONSTRAINT CK_Clients_FirstName_NotEmpty CHECK (LEN(RTRIM(LTRIM(FirstName))) > 0),
    CONSTRAINT CK_Clients_LastName_NotEmpty CHECK (LEN(RTRIM(LTRIM(LastName))) > 0),
    CONSTRAINT CK_Clients_Email_NotEmpty CHECK (LEN(RTRIM(LTRIM(Email))) > 0),
    CONSTRAINT CK_Clients_Email_Format CHECK (Email LIKE '%_@__%.__%'),
    CONSTRAINT CK_Clients_DateOfBirth_NotFuture CHECK (DateOfBirth < CAST(GETUTCDATE() AS DATE)),
    CONSTRAINT CK_Clients_DateOfBirth_Realistic CHECK (DateOfBirth >= DATEADD(YEAR, -150, GETUTCDATE())),
    CONSTRAINT UQ_Clients_Email UNIQUE (Email)
);
GO

-- Index for performance on email lookups
CREATE NONCLUSTERED INDEX IX_Clients_Email ON Clients(Email);
GO

-- =============================================
-- Create Policies table with constraints
-- =============================================
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
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),

    -- Foreign key with proper referential integrity
    CONSTRAINT FK_Policies_Clients
        FOREIGN KEY (ClientId)
        REFERENCES Clients(Id)
        ON DELETE NO ACTION,  -- Changed from CASCADE to prevent accidental deletion
    
    -- Edge case constraints
    CONSTRAINT CK_Policies_PolicyNumber_NotEmpty CHECK (LEN(RTRIM(LTRIM(PolicyNumber))) > 0),
    CONSTRAINT CK_Policies_PolicyType_NotEmpty CHECK (LEN(RTRIM(LTRIM(PolicyType))) > 0),
    CONSTRAINT CK_Policies_PolicyType_Valid CHECK (PolicyType IN ('Health', 'Car', 'Home', 'Life', 'Travel', 'Property', 'Liability')),
    CONSTRAINT CK_Policies_Premium_Positive CHECK (PremiumAmount > 0),
    CONSTRAINT CK_Policies_Premium_Realistic CHECK (PremiumAmount <= 999999999999.99),
    CONSTRAINT CK_Policies_StartBeforeEnd CHECK (StartDate < EndDate),
    CONSTRAINT CK_Policies_MinimumDuration CHECK (DATEDIFF(DAY, StartDate, EndDate) >= 1),
    CONSTRAINT CK_Policies_MaximumDuration CHECK (DATEDIFF(YEAR, StartDate, EndDate) <= 10),
    CONSTRAINT UQ_Policies_PolicyNumber UNIQUE (PolicyNumber)
);
GO

-- Indexes for performance
CREATE NONCLUSTERED INDEX IX_Policies_ClientId ON Policies(ClientId);
CREATE NONCLUSTERED INDEX IX_Policies_PolicyNumber ON Policies(PolicyNumber);
CREATE NONCLUSTERED INDEX IX_Policies_IsActive ON Policies(IsActive);
CREATE NONCLUSTERED INDEX IX_Policies_DateRange ON Policies(StartDate, EndDate);
GO

-- =============================================
-- Optional: Trigger to prevent deletion of clients with active policies
-- =============================================
CREATE TRIGGER TR_Clients_PreventDeleteWithActivePolicies
ON Clients
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if any deleted client has active policies
    IF EXISTS (
        SELECT 1
        FROM deleted d
        INNER JOIN Policies p ON d.Id = p.ClientId
        WHERE p.IsActive = 1
    )
    BEGIN
        RAISERROR('Cannot delete client with active policies. Please deactivate or cancel policies first.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    -- If no active policies, allow deletion
    DELETE FROM Clients
    WHERE Id IN (SELECT Id FROM deleted);
END;
GO

-- =============================================
-- Optional: View for policy validation checks
-- =============================================
CREATE VIEW vw_PolicyValidation
AS
SELECT 
    p.Id,
    p.PolicyNumber,
    p.ClientId,
    c.FirstName + ' ' + c.LastName AS ClientName,
    p.PolicyType,
    p.PremiumAmount,
    p.StartDate,
    p.EndDate,
    p.IsActive,
    DATEDIFF(DAY, p.StartDate, p.EndDate) AS DurationDays,
    CASE 
        WHEN p.EndDate < CAST(GETUTCDATE() AS DATE) THEN 'Expired'
        WHEN p.StartDate > CAST(GETUTCDATE() AS DATE) THEN 'Future'
        WHEN p.IsActive = 1 THEN 'Active'
        ELSE 'Inactive'
    END AS PolicyStatus,
    CASE 
        WHEN p.StartDate >= p.EndDate THEN 'Invalid: Start >= End'
        WHEN DATEDIFF(DAY, p.StartDate, p.EndDate) < 1 THEN 'Invalid: Duration < 1 day'
        WHEN p.PremiumAmount <= 0 THEN 'Invalid: Premium <= 0'
        ELSE 'Valid'
    END AS ValidationStatus
FROM Policies p
INNER JOIN Clients c ON p.ClientId = c.Id;
GO
