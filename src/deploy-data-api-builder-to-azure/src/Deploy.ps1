#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$ResourceGroup = "rg-sampledabapi-dev-japaneast-001"
$Location = "japaneast"

az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

az deployment group create `
  --name "SampleDabApp.Database" `
  --resource-group $ResourceGroup `
  --template-file Database.bicep `
  --parameters Development.bicepparam `
  --output table

# $connectionString = az deployment group show `
#   --resource-group $ResourceGroup `
#   --name "SampleDabApp.Database" `
#   --query properties.outputs.connectionString.value

# TODO: なぜかエラーする。手動でコマンド打つと実行できる。
# Invoke-Sqlcmd `
#   -ConnectionString $connectionString `
#   -InputFile ./CreateTable.sql

az provider register --namespace "Microsoft.App"
az provider register --namespace "Microsoft.ContainerService"
az deployment group create `
  --name "SampleDabApp.Application" `
  --resource-group $ResourceGroup `
  --template-file Application.bicep `
  --parameters Application.bicepparam `
  --output table
