DROP DATABASE IF EXISTS [AdventureWorks2025];
GO

RESTORE DATABASE [AdventureWorks2025]
FROM DISK = N'/data/AdventureWorks2025.bak'
WITH
  MOVE 'AdventureWorks' TO '/var/opt/mssql/data/AdventureWorks2025.mdf',
  MOVE 'AdventureWorks_log' TO '/var/opt/mssql/data/AdventureWorks2025_log.ldf',
  FILE = 1,
  NOUNLOAD,
  STATS = 5;
GO
