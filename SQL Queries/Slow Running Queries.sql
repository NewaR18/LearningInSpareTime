--FIND SLOW RUNNING QUERY
SELECT s.session_id
    ,r.STATUS
    ,r.blocking_session_id 'blocked by' 
    ,r.wait_type
    ,wait_resource
    ,r.wait_time / (1000.0) 'Wait Time (in Sec)'
    ,r.total_elapsed_time / (1000.0) 'Elapsed Time (in Sec)',
	Quotename(Object_name(st.objectid, st.dbid)) PROC_NAME
    ,Substring(st.TEXT, (r.statement_start_offset / 2) + 1, (
            (
                CASE r.statement_end_offset
                    WHEN - 1
                        THEN Datalength(st.TEXT)
                    ELSE r.statement_end_offset
                    END - r.statement_start_offset
                ) / 2
            ) + 1) AS statement_text
    ,Coalesce(Quotename(Db_name(st.dbid)) + N'.' + Quotename(Object_schema_name(st.objectid, st.dbid)) + N'.' + 
Quotename(Object_name(st.objectid, st.dbid)), '') AS command_text
    ,r.cpu_time
    ,r.logical_reads
    ,r.reads
    ,r.writes
    ,r.command
    ,s.login_name
    ,s.host_name
    ,s.program_name
    ,s.host_process_id
    ,s.last_request_end_time
    ,s.login_time 
    ,r.open_transaction_count
FROM sys.dm_exec_sessions AS s
INNER JOIN sys.dm_exec_requests AS r ON r.session_id = s.session_id
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) AS st

WHERE r.session_id != @@SPID
ORDER BY r.cpu_time DESC
    ,r.STATUS
    ,r.blocking_session_id
    ,s.session_id

-- CHECK THIS ALSO
 
SELECT 
(CASE es.transaction_isolation_level 
		WHEN 0 THEN 'Unspecified' 
		WHEN 1 THEN 'ReadUncommitted' 
		WHEN 2 THEN 'ReadCommitted' 
		WHEN 3 THEN 'Repeatable' 
		WHEN 4 THEN 'Serializable' 
		WHEN 5 THEN 'Snapshot' 
	END) TRANSACTION_ISOLATION_LEVEL 
,OBJECT_NAME(sqltext.objectid) OBJECT_NAME,
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
left outer join sys.dm_exec_sessions es on es.session_id=req.session_id
ORDER BY req.total_elapsed_time DESC


