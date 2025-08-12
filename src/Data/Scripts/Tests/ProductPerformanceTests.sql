-- Product Performance Tests
-- These tests measure the performance of various product-related operations
SET NOCOUNT ON;
DECLARE @StartTime DATETIME, @EndTime DATETIME;

-- Test 1: Basic count of all records
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM Products;
SET @EndTime = GETDATE();
PRINT 'Test 1: Basic COUNT(*) Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 2: Product filtering performance
SET @StartTime = GETDATE();
SELECT TOP 1000 * FROM Products
WHERE Price > 50.00 AND Price < 200.00;
SET @EndTime = GETDATE();
PRINT 'Test 2: Price filtering Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 3: Product sorting performance
SET @StartTime = GETDATE();
SELECT TOP 1000 * FROM Products 
ORDER BY Price DESC;
SET @EndTime = GETDATE();
PRINT 'Test 3: Sorting Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 4: Product aggregation performance
SET @StartTime = GETDATE();
SELECT AVG(Price) as AveragePrice, MIN(Price) as MinPrice, 
       MAX(Price) as MaxPrice, COUNT(*) as ProductCount
FROM Products;
SET @EndTime = GETDATE();
PRINT 'Test 4: Aggregation Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 5: Product search performance
DECLARE @SearchTerm NVARCHAR(50) = '%abc%';
SET @StartTime = GETDATE();
SELECT TOP 1000 * FROM Products
WHERE ProductName LIKE @SearchTerm;
SET @EndTime = GETDATE();
PRINT 'Test 5: Search LIKE Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 6: Top 10 highest and lowest price products
SET @StartTime = GETDATE();
SELECT 'Highest Price' AS Category, ProductName, Price
FROM (
    SELECT TOP 10 ProductName, Price 
    FROM Products 
    ORDER BY Price DESC
) AS HighPriced
UNION ALL
SELECT 'Lowest Price' AS Category, ProductName, Price
FROM (
    SELECT TOP 10 ProductName, Price 
    FROM Products 
    ORDER BY Price ASC
) AS LowPriced
ORDER BY Category, Price DESC;
SET @EndTime = GETDATE();
PRINT 'Test 6: Price Extremes Query Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 7: Top 10 oldest and newest products
SET @StartTime = GETDATE();
SELECT 'Newest Products' AS Category, ProductName, CreatedAt
FROM (
    SELECT TOP 10 ProductName, CreatedAt 
    FROM Products 
    ORDER BY CreatedAt DESC
) AS NewestProducts
UNION ALL
SELECT 'Oldest Products' AS Category, ProductName, CreatedAt
FROM (
    SELECT TOP 10 ProductName, CreatedAt 
    FROM Products 
    ORDER BY CreatedAt ASC
) AS OldestProducts
ORDER BY Category, CreatedAt DESC;
SET @EndTime = GETDATE();
PRINT 'Test 7: Product Age Extremes Query Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';