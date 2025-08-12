USE LoadTestingDb
GO

-- Create table to store FK information for recreation
PRINT 'Saving foreign key information...'
IF OBJECT_ID('tempdb..#ForeignKeys') IS NOT NULL
    DROP TABLE #ForeignKeys;

CREATE TABLE #ForeignKeys (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FK_Name NVARCHAR(255),
    Parent_Table NVARCHAR(255),
    Parent_Column NVARCHAR(255),
    Referenced_Table NVARCHAR(255),
    Referenced_Column NVARCHAR(255)
);

-- Capture foreign key information
INSERT INTO #ForeignKeys
SELECT 
    OBJECT_NAME(fk.object_id) AS FK_Name,
    OBJECT_NAME(fk.parent_object_id) AS Parent_Table,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS Parent_Column,
    OBJECT_NAME(fk.referenced_object_id) AS Referenced_Table,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS Referenced_Column
FROM 
    sys.foreign_keys AS fk
    INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY 
    fk.name;

-- Drop foreign keys
PRINT 'Dropping foreign keys...'
DECLARE @sql NVARCHAR(MAX);
DECLARE @fkName NVARCHAR(255);
DECLARE @parentTable NVARCHAR(255);

DECLARE curFK CURSOR FOR
    SELECT FK_Name, Parent_Table FROM #ForeignKeys;

OPEN curFK;
FETCH NEXT FROM curFK INTO @fkName, @parentTable;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = 'ALTER TABLE [' + @parentTable + '] DROP CONSTRAINT [' + @fkName + ']';
    EXEC sp_executesql @sql;
    FETCH NEXT FROM curFK INTO @fkName, @parentTable;
END

CLOSE curFK;
DEALLOCATE curFK;

-- Truncate tables (now possible since FKs are gone)
PRINT 'Truncating tables...'
TRUNCATE TABLE OrderItems
TRUNCATE TABLE Orders
TRUNCATE TABLE Products
TRUNCATE TABLE Customers

-- Recreate foreign keys
PRINT 'Recreating foreign keys...'
DECLARE @parentColumn NVARCHAR(255);
DECLARE @referencedTable NVARCHAR(255);
DECLARE @referencedColumn NVARCHAR(255);

DECLARE curRecreateFK CURSOR FOR
    SELECT FK_Name, Parent_Table, Parent_Column, Referenced_Table, Referenced_Column 
    FROM #ForeignKeys;

OPEN curRecreateFK;
FETCH NEXT FROM curRecreateFK INTO @fkName, @parentTable, @parentColumn, @referencedTable, @referencedColumn;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = 'ALTER TABLE [' + @parentTable + '] ADD CONSTRAINT [' + 
               @fkName + '] FOREIGN KEY([' + @parentColumn + ']) REFERENCES [' + 
               @referencedTable + ']([' + @referencedColumn + '])';
    EXEC sp_executesql @sql;
    FETCH NEXT FROM curRecreateFK INTO @fkName, @parentTable, @parentColumn, @referencedTable, @referencedColumn;
END

CLOSE curRecreateFK;
DEALLOCATE curRecreateFK;

-- Clean up
DROP TABLE #ForeignKeys;

PRINT 'All tables truncated successfully and foreign keys recreated.';
GO