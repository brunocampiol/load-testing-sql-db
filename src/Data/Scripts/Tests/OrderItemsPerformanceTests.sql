-- Performance tests for OrderItems table
SET NOCOUNT ON;
DECLARE @StartTime DATETIME, @EndTime DATETIME;

-- Test 1: Basic count of all order items
SET @StartTime = GETDATE();
SELECT COUNT(*) FROM OrderItems;
SET @EndTime = GETDATE();
PRINT 'Test 1: Basic COUNT(*) Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 2: Count of order items per order
SET @StartTime = GETDATE();
SELECT TOP 1000 OrderId, COUNT(*) AS ItemCount
FROM OrderItems
GROUP BY OrderId
ORDER BY ItemCount DESC;
SET @EndTime = GETDATE();
PRINT 'Test 2: Items per Order Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 3: Total quantity per product
SET @StartTime = GETDATE();
SELECT TOP 1000 oi.ProductId, p.ProductName, SUM(oi.Quantity) AS TotalQuantity
FROM OrderItems oi
JOIN Products p ON oi.ProductId = p.ID
GROUP BY oi.ProductId, p.ProductName
ORDER BY TotalQuantity DESC;
SET @EndTime = GETDATE();
PRINT 'Test 3: Product Quantity Totals Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 4: Complex join across all tables
SET @StartTime = GETDATE();
SELECT TOP 1000 c.Name AS CustomerName, p.ProductName, SUM(oi.Quantity) AS TotalQuantity
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
JOIN Customers c ON o.CustomerId = c.ID
JOIN Products p ON oi.ProductId = p.ID
GROUP BY c.Name, p.ProductName
ORDER BY TotalQuantity DESC;
SET @EndTime = GETDATE();
PRINT 'Test 4: Complex Join Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 5: Specific order item lookup
DECLARE @RandomOrderItemId UNIQUEIDENTIFIER;
SELECT TOP 1 @RandomOrderItemId = ID FROM OrderItems ORDER BY NEWID();
SET @StartTime = GETDATE();
SELECT oi.*, p.ProductName, p.Price, o.CreatedAt AS OrderDate
FROM OrderItems oi
JOIN Products p ON oi.ProductId = p.ID
JOIN Orders o ON oi.OrderId = o.ID
WHERE oi.ID = @RandomOrderItemId;
SET @EndTime = GETDATE();
PRINT 'Test 5: Order Item GUID Lookup Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 6: Total order value calculation
SET @StartTime = GETDATE();
SELECT TOP 1000 o.ID AS OrderId, c.Name AS CustomerName, 
       SUM(oi.Quantity * p.Price) AS OrderTotal
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
JOIN Customers c ON o.CustomerId = c.ID
JOIN Products p ON oi.ProductId = p.ID
GROUP BY o.ID, c.Name
ORDER BY OrderTotal DESC;
SET @EndTime = GETDATE();
PRINT 'Test 6: Order Value Calculation Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 7: Orders with high quantities
SET @StartTime = GETDATE();
SELECT TOP 1000 oi.OrderId, o.CreatedAt, 
       COUNT(DISTINCT oi.ProductId) AS UniqueProducts, 
       SUM(oi.Quantity) AS TotalItems
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
GROUP BY oi.OrderId, o.CreatedAt
HAVING SUM(oi.Quantity) > 5
ORDER BY TotalItems DESC;
SET @EndTime = GETDATE();
PRINT 'Test 7: High Quantity Orders Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 8: Product distribution in orders
SET @StartTime = GETDATE();
WITH ProductOrderStats AS (
    SELECT
        p.ID AS ProductId,
        p.ProductName,
        COUNT(DISTINCT oi.OrderId) AS OrderCount,
        SUM(oi.Quantity) AS TotalQuantity
    FROM Products p
    LEFT JOIN OrderItems oi ON p.ID = oi.ProductId
    GROUP BY p.ID, p.ProductName
)
SELECT TOP 1000
    ProductName,
    OrderCount,
    TotalQuantity,
    CASE 
        WHEN OrderCount = 0 THEN 'Never ordered'
        WHEN OrderCount < 5 THEN 'Rarely ordered'
        WHEN OrderCount < 20 THEN 'Occasionally ordered'
        ELSE 'Frequently ordered'
    END AS OrderFrequency
FROM ProductOrderStats
ORDER BY OrderCount DESC, TotalQuantity DESC;
SET @EndTime = GETDATE();
PRINT 'Test 8: Product Distribution Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 9: Most recent order items
SET @StartTime = GETDATE();
SELECT TOP 1000 oi.ID, oi.OrderId, p.ProductName, oi.Quantity, p.Price, o.CreatedAt
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
JOIN Products p ON oi.ProductId = p.ID
ORDER BY o.CreatedAt DESC;
SET @EndTime = GETDATE();
PRINT 'Test 9: Recent Order Items Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 10: Items from orders in the last month
SET @StartTime = GETDATE();
SELECT TOP 1000 p.ProductName, COUNT(oi.ID) AS TimesOrdered, SUM(oi.Quantity) AS TotalQuantity
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
JOIN Products p ON oi.ProductId = p.ID
WHERE o.CreatedAt >= DATEADD(MONTH, -1, GETDATE())
GROUP BY p.ProductName
ORDER BY TotalQuantity DESC;
SET @EndTime = GETDATE();
PRINT 'Test 10: Last Month Items Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 11: Average items per order
SET @StartTime = GETDATE();
WITH OrderItemCounts AS (
    SELECT OrderId, COUNT(*) AS ItemCount, SUM(Quantity) AS TotalQuantity
    FROM OrderItems
    GROUP BY OrderId
)
SELECT 
    AVG(ItemCount * 1.0) AS AvgUniqueItemsPerOrder,
    AVG(TotalQuantity * 1.0) AS AvgQuantityPerOrder,
    MIN(ItemCount) AS MinItemsPerOrder,
    MAX(ItemCount) AS MaxItemsPerOrder,
    MIN(TotalQuantity) AS MinQuantityPerOrder,
    MAX(TotalQuantity) AS MaxQuantityPerOrder
FROM OrderItemCounts;
SET @EndTime = GETDATE();
PRINT 'Test 11: Average Items Per Order Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 12: Customers who bought specific product
DECLARE @RandomProductId UNIQUEIDENTIFIER;
SELECT TOP 1 @RandomProductId = ID FROM Products ORDER BY NEWID();
DECLARE @RandomProductName NVARCHAR(255);
SELECT @RandomProductName = ProductName FROM Products WHERE ID = @RandomProductId;

SET @StartTime = GETDATE();
SELECT TOP 1000 c.ID, c.Name
FROM Customers c
WHERE EXISTS (
    SELECT 1
    FROM Orders o
    JOIN OrderItems oi ON o.ID = oi.OrderId
    WHERE o.CustomerId = c.ID
    AND oi.ProductId = @RandomProductId
)
ORDER BY c.Name;
SET @EndTime = GETDATE();
PRINT 'Test 12: Customers Who Bought "' + @RandomProductName + '" Duration: ' + 
      CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 13: Top 10 products that appear multiple times in the same order
SET @StartTime = GETDATE();
WITH DuplicateProductsInOrders AS (
    SELECT 
        oi.ProductId,
        p.ProductName,
        oi.OrderId,
        COUNT(*) AS TimesInSameOrder
    FROM OrderItems oi
    JOIN Products p ON oi.ProductId = p.ID
    GROUP BY oi.ProductId, p.ProductName, oi.OrderId
    HAVING COUNT(*) > 1
),
ProductDuplicateStats AS (
    SELECT 
        ProductId,
        ProductName,
        COUNT(*) AS OrdersWithDuplicates,
        SUM(TimesInSameOrder) AS TotalDuplicateInstances,
        AVG(TimesInSameOrder * 1.0) AS AvgDuplicatesPerOrder,
        MAX(TimesInSameOrder) AS MaxDuplicatesInOneOrder
    FROM DuplicateProductsInOrders
    GROUP BY ProductId, ProductName
)
SELECT TOP 10
    ProductName,
    OrdersWithDuplicates,
    TotalDuplicateInstances,
    AvgDuplicatesPerOrder,
    MaxDuplicatesInOneOrder
FROM ProductDuplicateStats
ORDER BY OrdersWithDuplicates DESC, TotalDuplicateInstances DESC;
SET @EndTime = GETDATE();
PRINT 'Test 13: Top 10 Products with Multiple Entries Per Order Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 14: Inventory availability check (common e-commerce scenario)
SET @StartTime = GETDATE();
SELECT TOP 1000 p.ID, p.ProductName, p.Price,
       COALESCE(SUM(oi.Quantity), 0) AS TotalSold,
       (100 - COALESCE(SUM(oi.Quantity), 0)) AS EstimatedStock
FROM Products p
LEFT JOIN OrderItems oi ON p.ID = oi.ProductId
GROUP BY p.ID, p.ProductName, p.Price
HAVING (100 - COALESCE(SUM(oi.Quantity), 0)) < 10  -- Low stock items
ORDER BY EstimatedStock ASC;
SET @EndTime = GETDATE();
PRINT 'Test 14: Low Stock Check Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 15: Customer purchase history pagination (common for user profiles)
DECLARE @RandomCustomerId UNIQUEIDENTIFIER;
SELECT TOP 1 @RandomCustomerId = ID FROM Customers ORDER BY NEWID();
SET @StartTime = GETDATE();
SELECT oi.ID, p.ProductName, oi.Quantity, p.Price, o.CreatedAt
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
JOIN Products p ON oi.ProductId = p.ID
WHERE o.CustomerId = @RandomCustomerId
ORDER BY o.CreatedAt DESC
OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;  -- Pagination
SET @EndTime = GETDATE();
PRINT 'Test 15: Customer Purchase History Pagination Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 16: Product recommendation based on order patterns (frequently bought together)
SET @StartTime = GETDATE();
WITH ProductPairs AS (
    SELECT 
        oi1.ProductId AS Product1,
        oi2.ProductId AS Product2,
        COUNT(*) AS TimesBoughtTogether
    FROM OrderItems oi1
    JOIN OrderItems oi2 ON oi1.OrderId = oi2.OrderId AND oi1.ProductId < oi2.ProductId
    GROUP BY oi1.ProductId, oi2.ProductId
    HAVING COUNT(*) >= 2
)
SELECT TOP 50 
    p1.ProductName AS Product1Name,
    p2.ProductName AS Product2Name,
    pp.TimesBoughtTogether
FROM ProductPairs pp
JOIN Products p1 ON pp.Product1 = p1.ID
JOIN Products p2 ON pp.Product2 = p2.ID
ORDER BY pp.TimesBoughtTogether DESC;
SET @EndTime = GETDATE();
PRINT 'Test 16: Frequently Bought Together Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 17: Customer segmentation by order value (business analytics)
SET @StartTime = GETDATE();
WITH CustomerOrderValues AS (
    SELECT 
        c.ID,
        c.Name,
        COUNT(DISTINCT o.ID) AS TotalOrders,
        SUM(oi.Quantity * p.Price) AS TotalSpent,
        AVG(oi.Quantity * p.Price) AS AvgOrderValue
    FROM Customers c
    JOIN Orders o ON c.ID = o.CustomerId
    JOIN OrderItems oi ON o.ID = oi.OrderId
    JOIN Products p ON oi.ProductId = p.ID
    GROUP BY c.ID, c.Name
)
SELECT 
    CASE 
        WHEN TotalSpent >= 1000 THEN 'High Value'
        WHEN TotalSpent >= 500 THEN 'Medium Value'
        WHEN TotalSpent >= 100 THEN 'Low Value'
        ELSE 'Minimal Value'
    END AS CustomerSegment,
    COUNT(*) AS CustomerCount,
    AVG(TotalSpent) AS AvgSpentInSegment,
    AVG(TotalOrders) AS AvgOrdersInSegment
FROM CustomerOrderValues
GROUP BY CASE 
    WHEN TotalSpent >= 1000 THEN 'High Value'
    WHEN TotalSpent >= 500 THEN 'Medium Value'
    WHEN TotalSpent >= 100 THEN 'Low Value'
    ELSE 'Minimal Value'
END
ORDER BY AvgSpentInSegment DESC;
SET @EndTime = GETDATE();
PRINT 'Test 17: Customer Segmentation Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 18: Peak shopping hours analysis (operational insights)
SET @StartTime = GETDATE();
SELECT 
    DATEPART(HOUR, o.CreatedAt) AS OrderHour,
    COUNT(oi.ID) AS ItemsOrdered,
    COUNT(DISTINCT o.ID) AS OrdersPlaced,
    COUNT(DISTINCT oi.ProductId) AS UniqueProductsOrdered,
    AVG(oi.Quantity * 1.0) AS AvgQuantityPerItem
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
WHERE o.CreatedAt >= DATEADD(DAY, -30, GETDATE())  -- Last 30 days
GROUP BY DATEPART(HOUR, o.CreatedAt)
ORDER BY ItemsOrdered DESC;
SET @EndTime = GETDATE();
PRINT 'Test 18: Peak Shopping Hours Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 19: Cart abandonment simulation (incomplete orders)
SET @StartTime = GETDATE();
WITH RecentOrders AS (
    SELECT DISTINCT OrderId
    FROM OrderItems oi
    JOIN Orders o ON oi.OrderId = o.ID
    WHERE o.CreatedAt >= DATEADD(DAY, -7, GETDATE())
),
OrderSizes AS (
    SELECT 
        ro.OrderId,
        COUNT(oi.ID) AS ItemCount,
        SUM(oi.Quantity) AS TotalQuantity
    FROM RecentOrders ro
    JOIN OrderItems oi ON ro.OrderId = oi.OrderId
    GROUP BY ro.OrderId
)
SELECT 
    CASE 
        WHEN ItemCount = 1 THEN 'Single Item'
        WHEN ItemCount <= 3 THEN 'Small Cart'
        WHEN ItemCount <= 7 THEN 'Medium Cart'
        ELSE 'Large Cart'
    END AS CartSize,
    COUNT(*) AS OrderCount,
    AVG(TotalQuantity * 1.0) AS AvgQuantity
FROM OrderSizes
GROUP BY CASE 
    WHEN ItemCount = 1 THEN 'Single Item'
    WHEN ItemCount <= 3 THEN 'Small Cart'
    WHEN ItemCount <= 7 THEN 'Medium Cart'
    ELSE 'Large Cart'
END
ORDER BY OrderCount DESC;
SET @EndTime = GETDATE();
PRINT 'Test 19: Cart Size Analysis Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 20: Product search simulation (partial name matching)
SET @StartTime = GETDATE();
SELECT TOP 100 p.ID, p.ProductName, p.Price,
       COALESCE(SUM(oi.Quantity), 0) AS TimesSold
FROM Products p
LEFT JOIN OrderItems oi ON p.ID = oi.ProductId
WHERE p.ProductName LIKE '%a%'  -- Common letter search
GROUP BY p.ID, p.ProductName, p.Price
ORDER BY TimesSold DESC, p.ProductName;
SET @EndTime = GETDATE();
PRINT 'Test 20: Product Search Simulation Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 21: Seasonal trends analysis (monthly performance)
SET @StartTime = GETDATE();
SELECT 
    YEAR(o.CreatedAt) AS OrderYear,
    MONTH(o.CreatedAt) AS OrderMonth,
    COUNT(DISTINCT o.ID) AS TotalOrders,
    COUNT(oi.ID) AS TotalItems,
    SUM(oi.Quantity) AS TotalQuantity,
    COUNT(DISTINCT oi.ProductId) AS UniqueProducts,
    COUNT(DISTINCT o.CustomerId) AS UniqueCustomers
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.ID
GROUP BY YEAR(o.CreatedAt), MONTH(o.CreatedAt)
ORDER BY OrderYear DESC, OrderMonth DESC;
SET @EndTime = GETDATE();
PRINT 'Test 21: Seasonal Trends Analysis Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 22: Bulk update simulation (price recalculation)
SET @StartTime = GETDATE();
SELECT 
    oi.ID,
    oi.OrderId,
    oi.ProductId,
    p.ProductName,
    oi.Quantity,
    p.Price AS CurrentPrice,
    (oi.Quantity * p.Price) AS LineTotal,
    (oi.Quantity * p.Price * 1.1) AS LineWithTax  -- Tax calculation
FROM OrderItems oi
JOIN Products p ON oi.ProductId = p.ID
WHERE oi.OrderId IN (
    SELECT TOP 1000 ID FROM Orders ORDER BY CreatedAt DESC
);
SET @EndTime = GETDATE();
PRINT 'Test 22: Bulk Price Recalculation Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 23: Customer lifetime value calculation
SET @StartTime = GETDATE();
WITH CustomerMetrics AS (
    SELECT 
        c.ID,
        c.Name,
        MIN(o.CreatedAt) AS FirstOrder,
        MAX(o.CreatedAt) AS LastOrder,
        COUNT(DISTINCT o.ID) AS TotalOrders,
        SUM(oi.Quantity * p.Price) AS TotalSpent,
        AVG(oi.Quantity * p.Price) AS AvgOrderValue,
        DATEDIFF(DAY, MIN(o.CreatedAt), MAX(o.CreatedAt)) AS CustomerLifespanDays
    FROM Customers c
    JOIN Orders o ON c.ID = o.CustomerId
    JOIN OrderItems oi ON o.ID = oi.OrderId
    JOIN Products p ON oi.ProductId = p.ID
    GROUP BY c.ID, c.Name
    HAVING COUNT(DISTINCT o.ID) > 1  -- Repeat customers only
)
SELECT TOP 100
    Name,
    TotalOrders,
    TotalSpent,
    AvgOrderValue,
    CustomerLifespanDays,
    CASE 
        WHEN CustomerLifespanDays > 0 
        THEN TotalSpent / (CustomerLifespanDays / 30.0)  -- Monthly spend rate
        ELSE TotalSpent 
    END AS MonthlySpendRate
FROM CustomerMetrics
ORDER BY TotalSpent DESC;
SET @EndTime = GETDATE();
PRINT 'Test 23: Customer Lifetime Value Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 24: Top products by order frequency
SET @StartTime = GETDATE();
SELECT TOP 100
    p.ProductName,
    p.Price,
    COUNT(DISTINCT oi.OrderId) AS OrderFrequency,
    SUM(oi.Quantity) AS TotalQuantitySold,
    AVG(oi.Quantity * 1.0) AS AvgQuantityPerOrder,
    SUM(oi.Quantity * p.Price) AS TotalRevenue
FROM Products p
JOIN OrderItems oi ON p.ID = oi.ProductId
GROUP BY p.ID, p.ProductName, p.Price
ORDER BY OrderFrequency DESC, TotalQuantitySold DESC;
SET @EndTime = GETDATE();
PRINT 'Test 24: Top Products by Order Frequency Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';

-- Test 25: Products never ordered (potential discontinued items)
SET @StartTime = GETDATE();
SELECT TOP 100
    p.ID,
    p.ProductName,
    p.Price,
    p.CreatedAt
FROM Products p
LEFT JOIN OrderItems oi ON p.ID = oi.ProductId
WHERE oi.ProductId IS NULL
ORDER BY p.CreatedAt DESC;
SET @EndTime = GETDATE();
PRINT 'Test 25: Products Never Ordered Duration: ' + CAST(DATEDIFF(MILLISECOND, @StartTime, @EndTime) AS VARCHAR) + ' ms';