
DECLARE @Counter INT = 0

WHILE @Counter < 3 -- Loop for three times
BEGIN
    -- Your backup log script here
    BACKUP LOG [INFINITY_032_001] TO DISK = 'Z:\LOG-Backup\INFINITY_032_001.trn'
    
    -- Your shrink file script here
USE [INFINITY_032_001]
DBCC SHRINKFILE (N'INFINITY_Log' , 100)
    
    -- Increment the counter
    SET @Counter = @Counter + 1
    
    -- Delay between iterations (optional)
    WAITFOR DELAY '00:00:30' -- 30 seconds delay, adjust as needed
END