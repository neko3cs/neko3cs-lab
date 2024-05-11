USE [master];
GO

DROP DATABASE IF EXISTS [AdventureWorksLT2022];
GO

RESTORE DATABASE [AdventureWorksLT2022]
FROM DISK = N'/data/AdventureWorksLT2022.bak'
WITH
  MOVE 'AdventureWorksLT2022_Data' TO '/var/opt/mssql/data/AdventureWorksLT2022.mdf',
  MOVE 'AdventureWorksLT2022_Log' TO '/var/opt/mssql/data/AdventureWorksLT2022_log.ldf',
  FILE = 1,
  NOUNLOAD,
  STATS = 5;
GO
