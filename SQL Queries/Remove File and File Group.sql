USE [INFINITY_055_001]
GO

--Find Filegroup
SELECT * FROM sys. filegroups

-- Find table
SELECT *
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='GLTRAN_DETL'
GO


USE [master]
GO
USE [INFINITY_055_001]
GO
ALTER DATABASE [INFINITY_055_001]  REMOVE FILE [TEST]
GO
ALTER DATABASE [INFINITY_055_001] REMOVE FILEGROUP [TEST]
GO