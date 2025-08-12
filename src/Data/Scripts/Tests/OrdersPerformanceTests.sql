-- Performance tests for Orders table
SET NOCOUNT ON;
DECLARE @StartTime DATETIME, @EndTime DATETIME;

-- Test 1: Basic count of all orders
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Orders;
SET @EndTime = GETDATE();
PRINT 'Test 1: Basic COUNT(*) Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 2: Filtered query on date range
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Orders 
WHERE CreatedAt BETWEEN '2023-01-01' AND '2023-06-30';
SET @EndTime = GETDATE();
PRINT 'Test 2: Date Range Filter Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 3: Aggregation by order month
SET @StartTime = GETDATE();
SELECT TOP 1000 YEAR(CreatedAt) AS OrderYear, MONTH(CreatedAt) AS OrderMonth, COUNT(*) AS OrderCount 
FROM Orders 
GROUP BY YEAR(CreatedAt), MONTH(CreatedAt)
ORDER BY OrderYear, OrderMonth;
SET @EndTime = GETDATE();
PRINT 'Test 3: Group By Order Month Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 4: Join with Customers
SET @StartTime = GETDATE();
SELECT TOP 1000 c.Name, COUNT(o.ID) AS OrderCount
FROM Orders o
JOIN Customers c ON o.CustomerId = c.ID
GROUP BY c.Name
ORDER BY OrderCount DESC;
SET @EndTime = GETDATE();
PRINT 'Test 4: Join with Customers Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 5: Recent orders
SET @StartTime = GETDATE();
SELECT TOP 1000 o.ID, o.CustomerId, c.Name AS CustomerName, o.CreatedAt
FROM Orders o
JOIN Customers c ON o.CustomerId = c.ID
ORDER BY o.CreatedAt DESC;
SET @EndTime = GETDATE();
PRINT 'Test 5: Recent Orders Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 6: Random sampling
SET @StartTime = GETDATE();
SELECT TOP 5 PERCENT o.ID, o.CustomerId, c.Name AS CustomerName, o.CreatedAt
FROM Orders o
JOIN Customers c ON o.CustomerId = c.ID
ORDER BY NEWID();
SET @EndTime = GETDATE();
PRINT 'Test 6: Random Sampling Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 7: Specific order lookup
DECLARE @RandomOrderId UNIQUEIDENTIFIER;
SELECT TOP 1 @RandomOrderId = ID FROM Orders ORDER BY NEWID();
SET @StartTime = GETDATE();
SELECT o.*, c.Name AS CustomerName 
FROM Orders o
JOIN Customers c ON o.CustomerId = c.ID
WHERE o.ID = @RandomOrderId;
SET @EndTime = GETDATE();
PRINT 'Test 7: Order GUID Lookup Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 8: Orders per customer
SET @StartTime = GETDATE();
SELECT TOP 1000 o.CustomerId, c.Name AS CustomerName, COUNT(*) AS OrderCount
FROM Orders o
JOIN Customers c ON o.CustomerId = c.ID
GROUP BY o.CustomerId, c.Name
ORDER BY OrderCount DESC;
SET @EndTime = GETDATE();
PRINT 'Test 8: Orders per Customer Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 9: Multi-condition filtering
SET @StartTime = GETDATE();
SELECT COUNT(*) 
FROM Orders o
WHERE o.CreatedAt > DATEADD(MONTH, -3, GETDATE())
AND EXISTS (SELECT 1 FROM Customers c WHERE c.ID = o.CustomerId AND c.DateOfBirth < '1990-01-01');
SET @EndTime = GETDATE();
PRINT 'Test 9: Complex Multi-Condition Query Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 10: Daily order volume
SET @StartTime = GETDATE();
SELECT TOP 1000 CAST(CreatedAt AS DATE) AS OrderDate, COUNT(*) AS DailyOrderCount
FROM Orders
WHERE CreatedAt > DATEADD(MONTH, -6, GETDATE())
GROUP BY CAST(CreatedAt AS DATE)
ORDER BY OrderDate;
SET @EndTime = GETDATE();
PRINT 'Test 10: Daily Order Volume Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 11: Customers with no orders
SET @StartTime = GETDATE();
SELECT TOP 1000 c.ID, c.Name
FROM Customers c
LEFT JOIN Orders o ON c.ID = o.CustomerId
WHERE o.ID IS NULL;
SET @EndTime = GETDATE();
PRINT 'Test 11: Customers with No Orders Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 12: Customer order distribution (none, single, multiple) with totals and percentages
SET @StartTime = GETDATE();
WITH CustomerOrderCounts AS (
    SELECT 
        c.ID AS CustomerId,
        COUNT(o.ID) AS OrderCount
    FROM Customers c
    LEFT JOIN Orders o ON c.ID = o.CustomerId
    GROUP BY c.ID
),
CustomerCounts AS (
    SELECT 
        1 AS SortOrder,
        'Customers with no orders' AS Category,
        COUNT(*) AS CustomerCount
    FROM CustomerOrderCounts
    WHERE OrderCount = 0

    UNION ALL

    SELECT 
        2 AS SortOrder,
        'Customers with exactly one order' AS Category,
        COUNT(*) AS CustomerCount
    FROM CustomerOrderCounts
    WHERE OrderCount = 1

    UNION ALL

    SELECT 
        3 AS SortOrder,
        'Customers with multiple orders' AS Category,
        COUNT(*) AS CustomerCount
    FROM CustomerOrderCounts
    WHERE OrderCount > 1
)
SELECT 
    cc.SortOrder,
    cc.Category,
    cc.CustomerCount,
    SUM(cc.CustomerCount) OVER () AS TotalCustomers,
    CAST(100.0 * cc.CustomerCount / SUM(cc.CustomerCount) OVER () AS DECIMAL(5,2)) AS Percentage
FROM CustomerCounts cc
ORDER BY cc.SortOrder;
SET @EndTime = GETDATE();
PRINT 'Test 12: Customer Order Distribution Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';