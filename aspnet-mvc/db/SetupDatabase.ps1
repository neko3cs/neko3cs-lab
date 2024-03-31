#Requires -PSEdition Core

if (-not (Test-Path $PWD/AdventureWorksLT2022.bak)) {
  curl -fsSOL https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorksLT2022.bak
}
docker-compose up --detach
Invoke-Sqlcmd `
  -ConnectionString 'Data Source=localhost;Initial Catalog=master;User ID=sa;Password=P@ssword!;' `
  -InputFile ./RestoreDatabase.sql
