SELECT
    tables.name AS TableName,
    indexes.name AS IndexName,
    SUM(dm_db_column_store_row_group_physical_stats.total_rows) 
        AS TotalRows,
    SUM(dm_db_column_store_row_group_physical_stats.deleted_rows) 
        AS DeletedRows,
	SUM(dm_db_column_store_row_group_physical_stats.deleted_rows)*100.00/
		SUM(dm_db_column_store_row_group_physical_stats.total_rows) ChangePercentage
FROM sys.dm_db_column_store_row_group_physical_stats
INNER JOIN sys.indexes
ON indexes.index_id = 
    dm_db_column_store_row_group_physical_stats.index_id
AND indexes.object_id = 
    dm_db_column_store_row_group_physical_stats.object_id
INNER JOIN sys.tables
ON tables.object_id = indexes.object_id
GROUP BY tables.name, indexes.name