USE LoadTestingDb
GO

-- Improves performance when retrieving all orders for a specific customer
-- Common query pattern in order management systems
-- Although SQL Server may create a non-clustered index due to the foreign key constraint, explicitly creating it ensures optimal performance
CREATE INDEX IX_Orders_CustomerId ON Orders(CustomerId);

-- Speeds up retrieval of all items within a specific order
-- This is a fundamental operation in any order system
-- Particularly beneficial as order details are frequently accessed
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);

-- Optimizes queries that filter orders by date range for specific customers
-- Supports common reporting scenarios (e.g., "all orders for customer X in the last month")
-- The column order (CreatedAt first, then CustomerId) allows the index to be used for date-only queries as well
CREATE INDEX IX_Orders_CreatedAt_CustomerId ON Orders(CreatedAt, CustomerId);