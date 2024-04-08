SELECT bs.database_name
, backuptype = CASE 
	WHEN bs.type = 'D' and bs.is_copy_only = 0 THEN 'Full Database'
	WHEN bs.type = 'D' and bs.is_copy_only = 1 THEN 'Full Copy-Only Database'
	WHEN bs.type = 'I' THEN 'Differential database backup'
	WHEN bs.type = 'L' THEN 'Transaction Log'
	WHEN bs.type = 'F' THEN 'File or filegroup'
	WHEN bs.type = 'G' THEN 'Differential file'
	WHEN bs.type = 'P' THEN 'Partial'
	WHEN bs.type = 'Q' THEN 'Differential partial' END + ' Backup'
, bs.recovery_model
, BackupStartDate = bs.Backup_Start_Date
, BackupFinishDate = bs.Backup_Finish_Date
, LatestBackupLocation = bf.physical_device_name
, backup_size_mb = bs.backup_size/1024./1024.
, compressed_backup_size_mb = bs.compressed_backup_size/1024./1024.
, database_backup_lsn -- For tlog and differential backups, this is the checkpoint_lsn of the FULL backup it is based on. 
, checkpoint_lsn
, begins_log_chain
FROM msdb.dbo.backupset bs	
LEFT OUTER JOIN msdb.dbo.backupmediafamily bf ON bs.[media_set_id] = bf.[media_set_id]
WHERE recovery_model in ('FULL', 'BULK-LOGGED')
AND bs.backup_start_date > DATEADD(month, -2, sysdatetime()) --only look at last two months
ORDER BY bs.database_name asc, bs.Backup_Start_Date desc;