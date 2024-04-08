SELECT t1.session_id, t1.request_id, t1.task_alloc,t1.task_dealloc,
	(SELECT SUBSTRING(text, t2.statement_start_offset/2 + 1,
		(CASE when statement_end_offset = -1
			THEN LEN(CONVERT(nvarchar(max),text)) * 2
			ELSE statement_end_offset 
			END - t2.statement_start_offset)/2)
	 FROM sys.dm_exec_sql_text(sql_handle)) query_TEXT,
	 (SELECT query_plan
	  FROM sys.dm_exec_query_plan(t2.plan_handle)) as query_plan
FROM
	(Select session_id, request_id,
		SUM(internal_objects_alloc_page_count+user_objects_alloc_page_count) as task_alloc,
		SUM(internal_objects_dealloc_page_count+user_objects_dealloc_page_count) as task_dealloc
	from sys.dm_db_task_space_usage
	group by session_id, request_id ) t1
		INNER JOIN sys.dm_exec_requests t2 
			ON t1.session_id=t2.session_id
				AND t1.request_id = t2.request_id
				--and t1.session_id > 50
Order By t1.task_alloc DESC

SELECT * FROM sys.dm_exec_sessions
where [host_name] <> null