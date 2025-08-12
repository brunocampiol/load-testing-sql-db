CREATE DATABASE LoadTestingDb
GO

USE LoadTestingDb
GO

-- Set the compatibility level to SQL Server 2022
ALTER DATABASE LoadTestingDb SET COMPATIBILITY_LEVEL = 160
GO

-- Configure the database options for performance
ALTER DATABASE LoadTestingDb 
SET RECOVERY SIMPLE, 
    AUTO_UPDATE_STATISTICS ON, 
    AUTO_UPDATE_STATISTICS_ASYNC ON
GO

-- Optional: Set the maximum size for the database
ALTER DATABASE LoadTestingDb 
MODIFY FILE (NAME = LoadTestingDb, MAXSIZE = UNLIMITED)
GO

-- Optional: Set the initial size for the database
ALTER DATABASE LoadTestingDb 
MODIFY FILE (NAME = LoadTestingDb, SIZE = 100MB)
GO