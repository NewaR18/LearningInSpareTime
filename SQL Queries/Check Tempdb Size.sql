-- Check Tempdb Size
SELECT instance_name AS 'Database',
[Data File(s) Size (KB)]/1024 AS [Data file (MB)],
[Log File(s) Size (KB)]/1024 AS [Log file (MB)],
[Log File(s) Used Size (KB)]/1024 AS [Log file space used (MB)]
FROM (SELECT * FROM sys.dm_os_performance_counters
WHERE counter_name IN
('Data File(s) Size (KB)',
'Log File(s) Size (KB)',
'Log File(s) Used Size (KB)')
AND instance_name = 'tempdb') AS A
PIVOT
(MAX(cntr_value) FOR counter_name IN
([Data File(s) Size (KB)],
[LOG File(s) Size (KB)],
[Log File(s) Used Size (KB)])) AS B
GO

SELECT 
(SUM(unallocated_extent_page_count)/128) AS [Free space (MB)],
SUM(internal_object_reserved_page_count)*8 AS [Internal objects (KB)],
SUM(user_object_reserved_page_count)*8 AS [User objects (KB)],
SUM(version_store_reserved_page_count)*8 AS [Version store (KB)]
FROM sys.dm_db_file_space_usage
--database_id '2' represents tempdb
WHERE database_id = 2
GO

--SELECT tb.name AS [Temporary table name],
--stt.row_count AS [Number of rows], 
--stt.used_page_count * 8 AS [Used space (KB)], 
--stt.reserved_page_count * 8 AS [Reserved space (KB)] FROM tempdb.sys.partitions AS prt 
--INNER JOIN tempdb.sys.dm_db_partition_stats AS stt 
--ON prt.partition_id = stt.partition_id 
--AND prt.partition_number = stt.partition_number 
--INNER JOIN tempdb.sys.tables AS tb 
--ON stt.object_id = tb.object_id 
--ORDER BY tb.name desc
GO


SELECT (SUM(unallocated_extent_page_count)*1.0/128) AS TempDB_FreeSpaceAmount_InMB 
		, (SUM(version_store_reserved_page_count)*1.0/128) AS TempDB_VersionStoreSpaceAmount_InMB
		, (SUM(internal_object_reserved_page_count)*1.0/128) AS TempDB_InternalObjSpaceAmount_InMB
		, (SUM(user_object_reserved_page_count)*1.0/128) AS TempDB_UserObjSpaceAmount_InMB
FROM sys.dm_db_file_space_usage;