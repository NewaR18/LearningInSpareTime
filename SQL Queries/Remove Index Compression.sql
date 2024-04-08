SELECT 
    s.name AS SchemaName,
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    CASE 
        WHEN p.data_compression = 0 THEN 'NONE'
        WHEN p.data_compression = 1 THEN 'ROW'
        WHEN p.data_compression = 2 THEN 'PAGE'
        ELSE 'UNKNOWN'
    END AS CompressionType
	, 'ALTER INDEX '+i.name+' ON '+s.name+'.'+OBJECT_NAME(i.object_id)+' REBUILD WITH (DATA_COMPRESSION = NONE);' [Remove Compression Script]
FROM 
    sys.indexes i
JOIN 
    sys.partitions p ON i.object_id = p.object_id AND i.index_id = p.index_id
JOIN
    sys.tables t ON i.object_id = t.object_id
JOIN
    sys.schemas s ON t.schema_id = s.schema_id
JOIN
    sys.data_spaces ds ON i.data_space_id = ds.data_space_id
WHERE 
    i.type_desc IN ('CLUSTERED', 'NONCLUSTERED') -- Only consider clustered and nonclustered indexes
    AND p.data_compression <> 0 -- Filter out indexes without compression
    AND ds.name = 'INDEX'; -- Replace 'YourFilegroupName' with the name of the filegroup you want to filter on


	
