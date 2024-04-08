SELECT DISTINCT
    idx.name AS IndexName,
    idx.type_desc AS IndexType, SUM(p.rows) [ROWS]
FROM 
    sys.indexes AS idx 
INNER JOIN 
    sys.index_columns AS ic ON idx.object_id = ic.object_id AND idx.index_id = ic.index_id
INNER JOIN 
    sys.columns AS col ON ic.object_id = col.object_id AND ic.column_id = col.column_id

	INNER JOIN sys.partitions AS p ON idx.object_id = p.object_id AND idx.index_id = p.index_id
WHERE 
    idx.object_id = OBJECT_ID('GLTRAN_DETL')
GROUP BY idx.name, idx.type_desc
