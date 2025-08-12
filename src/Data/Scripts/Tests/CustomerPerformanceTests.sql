-- Performance tests for Customers table
SET NOCOUNT ON;
DECLARE @StartTime DATETIME, @EndTime DATETIME;

-- Test 1: Basic count of all records
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Customers;
SET @EndTime = GETDATE();
PRINT 'Test 1: Basic COUNT(*) Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 2: Filtered query on date range
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Customers 
WHERE DateOfBirth BETWEEN '1980-01-01' AND '1990-12-31';
SET @EndTime = GETDATE();
PRINT 'Test 2: Date Range Filter Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 3: Name search with LIKE
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Customers 
WHERE Name LIKE 'J%';
SET @EndTime = GETDATE();
PRINT 'Test 3: Name LIKE Search Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 4: Aggregation by birth year
SET @StartTime = GETDATE();
SELECT TOP 1000 YEAR(DateOfBirth) AS BirthYear, COUNT(*) AS CustomerCount 
FROM Customers 
GROUP BY YEAR(DateOfBirth)
ORDER BY BirthYear;
SET @EndTime = GETDATE();
PRINT 'Test 4: Group By Birth Year Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 5: Complex multi-condition query
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Customers 
WHERE DateOfBirth > '1990-01-01'
AND Name LIKE '%son%'
AND LEN(Name) > 10;
SET @EndTime = GETDATE();
PRINT 'Test 5: Complex Query Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 6: Top customers by most recent creation
SET @StartTime = GETDATE();
SELECT TOP 1000 ID, Name, DateOfBirth 
FROM Customers 
ORDER BY CreatedAt DESC;
SET @EndTime = GETDATE();
PRINT 'Test 6: TOP with ORDER BY Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 7: Random sampling
SET @StartTime = GETDATE();
SELECT TOP 1000 ID, Name 
FROM Customers 
ORDER BY NEWID();
SET @EndTime = GETDATE();
PRINT 'Test 7: Random Sampling Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 8: Query with specific GUID lookup (simulate lookup by ID)
DECLARE @RandomId UNIQUEIDENTIFIER;
SELECT TOP 1 @RandomId = ID FROM Customers ORDER BY NEWID();
SET @StartTime = GETDATE();
SELECT * FROM Customers WHERE ID = @RandomId;
SET @EndTime = GETDATE();
PRINT 'Test 8: GUID Lookup Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';