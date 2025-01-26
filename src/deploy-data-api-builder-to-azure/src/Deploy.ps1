#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$ResourceGroup = "rg-sampledabapi-dev-japaneast-001"
$Location = "japaneast"

az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

$DatabaseDeployName = "SampleDabApp.Database-$(Get-Date -Format 'yyyyMMddHHmmss')"
az deployment group create `
  --name $DatabaseDeployName `
  --resource-group $ResourceGroup `
  --template-file Database.bicep `
  --parameters Development.bicepparam `
  --output table

$connectionString = az deployment group show `
  --resource-group $ResourceGroup `
  --name $DatabaseDeployName `
  --query properties.outputs.connectionString.value

Invoke-Sqlcmd `
  -ConnectionString $connectionString `
  -InputFile ./CreateTable.sql

# az provider register --namespace "Microsoft.App"
# az provider register --namespace "Microsoft.ContainerService"
# az deployment group create `
#   --name "SampleDabApp.Application-$(Get-Date -Format 'yyyyMMddHHmmss')" `
#   --resource-group $ResourceGroup `
#   --template-file Application.bicep `
#   --parameters Development.bicepparam `
#   --output table
