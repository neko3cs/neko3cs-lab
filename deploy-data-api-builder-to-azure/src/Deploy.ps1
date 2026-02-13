#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

# 設定値
$ResourceGroup = "rg-sampledabapi-dev-japaneast-001"
$Location = "japaneast"
$AppName = "sampledabapp"

# データベース設定
$SqlDBServerName = "sample-app-sqldatabase-server"
$SqlDBName = "SampleAppDatabase"
$SqlAdminLogin = "db_admin"
$SqlAdminPassword = "P@ssword!"
$SqlPublicIpAddress = "*"

# ストレージ設定
$StorageAccountName = "samplestorage" + (Get-Random -Minimum 10000 -Maximum 100000)
$FileShareName = "dabconfig"
$DabConfigFileName = "dab-config.json"

# リソースグループの作成
Write-Output "Creating resource group '$ResourceGroup'..."
az group create --name $ResourceGroup --location $Location --output table

# データベースのデプロイ
Write-Output "Deploying SQL Database..."
az deployment group create `
  --name "SampleDabApp.Database" `
  --resource-group $ResourceGroup `
  --template-file Database.bicep `
  --parameters `
    serverName=$SqlDBServerName `
    sqlDBName=$SqlDBName `
    adminLogin=$SqlAdminLogin `
    adminLoginPassword=$SqlAdminPassword `
    publicIpAddress=$SqlPublicIpAddress `
  --output table

$connectionString = az deployment group show `
  --resource-group $ResourceGroup `
  --name "SampleDabApp.Database" `
  --query properties.outputs.connectionString.value -o tsv

$connectionString | Out-File -FilePath "connectionString.txt" -Encoding utf8
Write-Output "Connection string saved to connectionString.txt"

# テーブルの作成
Write-Output "Creating database table..."
Invoke-Sqlcmd -ConnectionString $connectionString -InputFile ./CreateTable.sql

# ストレージのデプロイ
Write-Output "Deploying Storage Account '$StorageAccountName'..."
az deployment group create `
  --name "SampleDabApp.Storage" `
  --resource-group $ResourceGroup `
  --template-file Storage.bicep `
  --parameters storageAccountName=$StorageAccountName fileShareName=$FileShareName `
  --output table

$storageAccountKey = $(az storage account keys list `
    -g $ResourceGroup `
    -n $StorageAccountName `
    --query '[0].value' -o tsv)

$storageConnectionString = $(az storage account show-connection-string `
    --name $StorageAccountName `
    -g $ResourceGroup `
    --query connectionString -o tsv)

# 設定ファイルのアップロード
Write-Output "Uploading configuration file '$DabConfigFileName'..."
az storage file upload `
  --source ./dab-config.json `
  --path $DabConfigFileName `
  --share-name $FileShareName `
  --connection-string $storageConnectionString --output table

# アプリケーションのデプロイ
Write-Output "Deploying Application (DAB on ACA)..."
az provider register --namespace "Microsoft.App"
az provider register --namespace "Microsoft.ContainerService"

az deployment group create `
  --name "SampleDabApp.Application" `
  --resource-group $ResourceGroup `
  --template-file Application.bicep `
  --parameters `
    appName=$AppName `
    connectionString="$connectionString" `
    storageAccountName=$StorageAccountName `
    storageAccountKey=$storageAccountKey `
    mountedStorageName=$FileShareName `
  --output table

$dabUrl = az deployment group show `
  --resource-group $ResourceGroup `
  --name "SampleDabApp.Application" `
  --query properties.outputs.dabUrl.value -o tsv

$dabUrl | Out-File -FilePath "dabUrl.txt" -Encoding utf8
Write-Output "DAB URL saved to dabUrl.txt: $dabUrl"

Write-Output "Deployment completed successfully."
