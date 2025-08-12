USE LoadTestingDb
GO

BULK INSERT Customers
FROM 'C:\Temp\users_data.csv'
WITH
(
    FORMAT = 'CSV',         -- Specify CSV format to handle quotes
    FIELDQUOTE = '"',       -- Specify the quote character
    FIELDTERMINATOR = ',',  
    ROWTERMINATOR = '\n',   
    FIRSTROW = 2,
    CODEPAGE = '65001'     -- UTF-8 encoding
);