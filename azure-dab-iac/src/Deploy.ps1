#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$Environ = Get-Content .env | ConvertFrom-StringData
$ResourceGroup = $Environ.RESOURCE_GROUP
$Location = $Environ.LOCATION
$AppName = $Environ.APP_NAME
$SQLServerName = "$AppName-sqldatabase-server"
$SQLDatabaseName = $Environ.SQL_DATABASE_NAME
$SQLServerLogin = $Environ.SQL_SERVER_LOGIN
$SQLServerLoginPassword = $Environ.SQL_SERVER_LOGIN_PASSWORD

$DatabaseBicepParameters = @{
  "serverName"         = @{ "value" = $SQLServerName };
  "publicIpAddress"    = @{ "value" = ((Invoke-RestMethod -Uri checkip.amazonaws.com) -replace '\n', '') };
  "sqlDBName"          = @{ "value" = $SQLDatabaseName };
  "adminLogin"         = @{ "value" = $SQLServerLogin };
  "adminLoginPassword" = @{ "value" = $SQLServerLoginPassword };
} | ConvertTo-Json -Compress |
ForEach-Object { $_ -replace '"', '\"' }

$ApplicationBicepParameters = @{
  "location" = @{ "value" = $Location };
  "appName"  = @{ "value" = $AppName };
} | ConvertTo-Json -Compress |
ForEach-Object { $_ -replace '"', '\"' }

az provider register --namespace "Microsoft.App"
az provider register --namespace "Microsoft.ContainerService"

az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./Database.bicep `
  --parameters $DatabaseBicepParameters `
  --output table

$connectionString = (az sql db show-connection-string `
    --server $SQLServerName `
    --name $SQLDatabaseName `
    --client ado.net) `
  -replace "<username>", $SQLServerLogin `
  -replace "<password>", $SQLServerLoginPassword `
  -replace "TrustServerCertificate=False", "TrustServerCertificate=True" `
  -replace '^\s*"(.*)"\s*$', '$1'

Invoke-Sqlcmd `
  -ConnectionString $connectionString `
  -InputFile ./CreateTable.sql

az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./Application.bicep `
  --parameters $ApplicationBicepParameters `
  --output table
