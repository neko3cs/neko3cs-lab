#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$Environ = Get-Content .env | ConvertFrom-StringData
$Location = $Environ.LOCATION
$ResourceGroup = $Environ.RESOURCE_GROUP
$AppName = $Environ.APP_NAME
$SQLServerName = "$AppName-sqldatabase-server"
$SQLDatabaseName = $Environ.SQL_DATABASE_NAME
$SQLServerLogin = $Environ.SQL_SERVER_LOGIN
$SQLServerLoginPassword = $Environ.SQL_SERVER_LOGIN_PASSWORD

$DatabaseBicepParameters = @{
  "location"           = @{ "value" = $Location };
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

Write-Output "`n#Creating resource group...`n"
az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

Write-Output "`n#Creating sql-database...`n"
az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./Database.bicep `
  --parameters $DatabaseBicepParameters

(az sql db show-connection-string `
  --server $SQLServerName `
  --name $SQLDatabaseName `
  --client ado.net) `
  -replace "<username>", $SQLServerLogin `
  -replace "<password>", $SQLServerLoginPassword `
  -replace '^\s*"(.*)"\s*$', '$1' |
Out-File `
  -FilePath .\connectionString.txt `
  -NoNewline `
  -Encoding utf8
Write-Output "`n#ConnectionString in connectionString.txt`n"

Invoke-Sqlcmd `
  -ConnectionString (Get-Content -Path .\connectionString.txt) `
  -InputFile ./CreateTable.sql

Write-Output "`n#Creating container app with application-gateway...`n"
az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./Application.bicep `
  --parameters $ApplicationBicepParameters
