USE [tempdb]
GO
/*
This script is used to shrink a database file in
increments until it reaches a target free space limit.
Run this script in the database with the file to be shrunk.
1. Set @DBFileName to the name of database file to shrink.
2. Set @TargetFreeMB to the desired file free space in MB after shrink.
3. Set @ShrinkIncrementMB to the increment to shrink file by in MB
4. Run the script
Comments	:	- This script will help shrink database in chunks.
                        - Remember : Shriking a database file should be done as a last practice.
                        - http://www.sqlskills.com/blogs/paul/why-you-should-not-shrink-your-data-files/
                        - https://www.brentozar.com/archive/2009/08/stop-shrinking-your-database-files-seriously-now/
*/                      
declare @DBFileName sysname
declare @TargetFreeMB int
declare @ShrinkIncrementMB int

DROP TABLE IF EXISTS #FINAL_table
DROP TABLE IF EXISTS #PREV_OLD_TABLE
DROP TABLE IF EXISTS #TEMPSYFILE

select
        [FileSizeMB]    =
                convert(numeric(10,2),round(a.size/128.,2)),
        [UsedSpaceMB]   =
                convert(numeric(10,2),round(fileproperty( a.name,'SpaceUsed')/128.,2)) ,
        [UnusedSpaceMB] =
                convert(numeric(10,2),round((a.size-fileproperty( a.name,'SpaceUsed'))/128.,2)) ,
        [DBFileName]    = a.name
INTO #FINAL_table 
from  sysfiles a
TRUNCATE TABLE #FINAL_table

SELECT * INTO #PREV_OLD_TABLE  FROM #FINAL_table

select [DBFileName]    = a.name INTO #TEMPSYFILE from  sysfiles a

WHILE EXISTS (SELECT TOP 1 1 FROM #TEMPSYFILE)
BEGIN
-- Set Name of Database file to shrink
set @DBFileName = (SELECT TOP 1 [DBFileName] FROM #TEMPSYFILE)  --<--- CHANGE HERE !!

-- Set Desired file free space in MB after shrink
set @TargetFreeMB = 1000			--<--- CHANGE HERE !!

-- Set Increment to shrink file by in MB
set @ShrinkIncrementMB = 500			--<--- CHANGE HERE !!

-- Show Size, Space Used, Unused Space, and Name of all database files --INSERT
INSERT INTO #PREV_OLD_TABLE ( [FileSizeMB],[UsedSpaceMB], [UnusedSpaceMB], [DBFileName])
select
		
        [FileSizeMB]    =
                convert(numeric(10,2),round(a.size/128.,2)),
        [UsedSpaceMB]   =
                convert(numeric(10,2),round(fileproperty( a.name,'SpaceUsed')/128.,2)) ,
        [UnusedSpaceMB] =
                convert(numeric(10,2),round((a.size-fileproperty( a.name,'SpaceUsed'))/128.,2)) ,
        [DBFileName]    = a.name
from
        sysfiles a
WHERE a.name = @DBFileName

declare @sql varchar(8000)
declare @SizeMB int
declare @UsedMB int

-- Get current file size in MB
select @SizeMB = size/128. from sysfiles where name = @DBFileName

-- Get current space used in MB
select @UsedMB = fileproperty( @DBFileName,'SpaceUsed')/128.

--select [StartFileSize] = @SizeMB, [StartUsedSpace] = @UsedMB, [DBFileName] = @DBFileName ,1

-- Loop until file at desired size
while  @SizeMB > @UsedMB+@TargetFreeMB+@ShrinkIncrementMB
        begin

        set @sql = 'dbcc shrinkfile ( '+@DBFileName+', '+ convert(varchar(20),@SizeMB-@ShrinkIncrementMB)+' ) '

        print 'Start ' + @sql
        print 'at '+convert(varchar(30),getdate(),121)

		exec ( @sql )

        print 'Done ' + @sql
        print 'at '+convert(varchar(30),getdate(),121)

        -- Get current file size in MB
        set @SizeMB = (select size/128. from sysfiles where name = @DBFileName)
        
        -- Get current space used in MB
        set @UsedMB = fileproperty( @DBFileName,'SpaceUsed')/128.

        --select [FileSize] = @SizeMB, [UsedSpace] = @UsedMB, [DBFileName] = @DBFileName
		
        end

		
--select [EndFileSize] = @SizeMB, [EndUsedSpace] = @UsedMB, [DBFileName] = @DBFileName

DELETE FROM #TEMPSYFILE WHERE [DBFileName]=@DBFileName

-- Show Size, Space Used, Unused Space, and Name of all database files
INSERT INTO #FINAL_table ([FileSizeMB], [UsedSpaceMB], [UnusedSpaceMB], [DBFileName])
select
        [FileSizeMB]    =
                convert(numeric(10,2),round(a.size/128.,2)),
        [UsedSpaceMB]   =
                convert(numeric(10,2),round(fileproperty( a.name,'SpaceUsed')/128.,2)) ,
        [UnusedSpaceMB] =
                convert(numeric(10,2),round((a.size-fileproperty( a.name,'SpaceUsed'))/128.,2)) ,
        [DBFileName]    = a.name
from
        sysfiles a
WHERE a.name = @DBFileName
END

SELECT '' [OLD] ,* FROM #PREV_OLD_TABLE
SELECT * FROM #FINAL_table
GO
