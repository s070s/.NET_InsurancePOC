-- 1) Insert Clients
INSERT INTO Clients (FirstName, LastName, Email, Phone, DateOfBirth)
VALUES
('John', 'Doe', 'john.doe@example.com', '+1-555-0101', '1985-06-15'),
('María', 'García', 'maria.garcia@example.es', '+34-600-111-222', '1990-03-22'),
('Sven', 'Müller', 'sven.mueller@example.de', '+49-30-9999-0000', '1972-11-05'),
('Anna', 'Petrova', 'anna.petrova@example.ru', '+7-495-123-4567', '1955-02-28'),
('Liam', 'O''Connor', 'liam.oconnor@example.ie', '+353-1-555-0000', '2000-12-01'),
('Zoe', 'Zhang', 'zoe.zhang@example.cn', '+86-10-8888-8888', '2010-05-10');

-- 2) Insert Policies (link by client email to obtain ClientId)
INSERT INTO Policies (ClientId, PolicyNumber, PolicyType, PremiumAmount, StartDate, EndDate, IsActive)
VALUES
((SELECT Id FROM Clients WHERE Email = 'john.doe@example.com'), 'CAR-10001', 'Car', 450.75, '2024-05-01', '2025-05-01', 1),
((SELECT Id FROM Clients WHERE Email = 'john.doe@example.com'), 'TRV-10002', 'Travel', 120.00, '2023-06-01', '2023-06-10', 0),
((SELECT Id FROM Clients WHERE Email = 'maria.garcia@example.es'), 'HLT-20001', 'Health', 3200.00, '2023-01-01', '2026-01-01', 1),
((SELECT Id FROM Clients WHERE Email = 'sven.mueller@example.de'), 'HME-30001', 'Home', 1500.00, '2016-01-01', '2026-01-01', 1),
((SELECT Id FROM Clients WHERE Email = 'sven.mueller@example.de'), 'LIA-30002', 'Liability', 75.00, '2025-12-01', '2025-12-02', 1),
((SELECT Id FROM Clients WHERE Email = 'anna.petrova@example.ru'), 'LFE-40001', 'Life', 5000.00, '2020-01-01', '2030-01-01', 1),
((SELECT Id FROM Clients WHERE Email = 'liam.oconnor@example.ie'), 'PRP-50001', 'Property', 2500.00, '2025-01-01', '2026-01-01', 1),
((SELECT Id FROM Clients WHERE Email = 'zoe.zhang@example.cn'), 'TRV-60001', 'Travel', 200.00, '2026-06-01', '2026-06-10', 1);

-- 3) Quick verification
SELECT Id, FirstName, LastName, Email, Phone, DateOfBirth, CreatedAt
FROM Clients
ORDER BY Id;

SELECT Id, PolicyNumber, ClientId, PolicyType, PremiumAmount, StartDate, EndDate, IsActive, CreatedAt
FROM Policies
ORDER BY Id;