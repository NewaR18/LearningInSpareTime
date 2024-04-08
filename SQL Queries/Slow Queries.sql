SELECT OBJECT_NAME(sqltext.objectid) OBJECT_NAME,
sqltext.TEXT,
req.session_id,
db.name,
req.status,
req.command,
req.cpu_time,
req.total_elapsed_time
FROM sys.dm_exec_requests req
inner join sys.databases db on db.database_id=req.database_id
CROSS APPLY sys.dm_exec_sql_text(sql_handle) AS sqltext
ORDER BY req.total_elapsed_time DESC


SELECT OBJECT_NAME(sqltext.objectid) OBJECT_NAME,  sqltext.TEXT,  req.session_id,  db.name,  req.status,  req.command,  req.cpu_time,  req.total_elapsed_time  FROM sys.dm_exec_requests req  inner join sys.databases db on db.database_id=req.database_id  CROSS APPLY sys.dm_exec_sql_text(sql_handle) AS sqltext  ORDER BY req.total_elapsed_time DESC