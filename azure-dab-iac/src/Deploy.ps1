#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$EnvData = Get-Content .env | ConvertFrom-StringData
$LOCATION = $EnvData.LOCATION
$RESOURCE_GROUP = $EnvData.RESOURCE_GROUP
$APP_NAME = $EnvData.APP_NAME
$SQL_DATABASE_NAME = $EnvData.SQL_DATABASE_NAME
$SQL_SERVER_LOGIN = $EnvData.SQL_SERVER_LOGIN
$SQL_SERVER_LOGIN_PASSWORD = $EnvData.SQL_SERVER_LOGIN_PASSWORD

$SQL_SERVER_NAME = "$APP_NAME-sqldatabase-server"
$PUBLIC_IP_ADDRESS = Invoke-RestMethod -Uri checkip.amazonaws.com

@(
  @{ ProviderName = "Microsoft.App" }
  @{ ProviderName = "Microsoft.ContainerService" }
) |
ForEach-Object {
  $Provider = az provider show --namespace $_.ProviderName |
  ConvertFrom-Json |
  Select-Object -First 1 -ExpandProperty registrationState |
  ForEach-Object { @{ IsRegisterd = ($_ -eq "Registered") } }

  if (-not $Provider.IsRegisterd) {
    az provider register --namespace $_.ProviderName
  }
}

Write-Output "`n#Creating resource group : '$RESOURCE_GROUP'`n"
az group create `
  --name $RESOURCE_GROUP `
  --location $LOCATION `
  --output table

Write-Output "`n#Creating sql-database : '$SQL_DATABASE_NAME' on the server : '$SQL_SERVER_NAME'`n"
az deployment group create `
  --resource-group $RESOURCE_GROUP `
  --template-file ./Database.bicep `
  --parameters `
  location="$LOCATION" `
  serverName="$SQL_SERVER_NAME" `
  publicIpAddress="$PUBLIC_IP_ADDRESS" `
  sqlDBName="$SQL_DATABASE_NAME" `
  adminLogin="$SQL_SERVER_LOGIN" `
  adminLoginPassword="$SQL_SERVER_LOGIN_PASSWORD"

$connectionString = (az sql db show-connection-string `
    --server $SQL_SERVER_NAME `
    --name $SQL_DATABASE_NAME `
    --client ado.net) `
  -replace "<username>", $SQL_SERVER_LOGIN `
  -replace "<password>", $SQL_SERVER_LOGIN_PASSWORD `
  -replace '^\s*"(.*)"\s*$', '$1'
Write-Output "`n#ConnectionString is: '$connectionString'`n"

Invoke-Sqlcmd `
  -ConnectionString $connectionString `
  -InputFile ./CreateTable.sql

Write-Output "`n#Creating container app with application-gateway: location='$LOCATION' appName='$APP_NAME'`n"
az deployment group create `
  --resource-group $RESOURCE_GROUP `
  --template-file ./Application.bicep `
  --parameters `
  location="$LOCATION" `
  appName="$APP_NAME"
