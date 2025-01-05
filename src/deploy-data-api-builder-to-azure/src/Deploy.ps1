#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$ResourceGroup = "rg-sampledabapi-dev-japaneast-001"
$Location = "japaneast"

# az provider register --namespace "Microsoft.App"
# az provider register --namespace "Microsoft.ContainerService"

az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

az deployment group create `
  --name "DatabaseDeploy" `
  --resource-group $ResourceGroup `
  --template-file ./Database.bicep `
  --parameters ./Development.bicepparam `
  --output table

$connectionString = az deployment group show `
  --resource-group $ResourceGroup `
  --name "DatabaseDeploy" `
  --query properties.outputs.connectionString.value

Invoke-Sqlcmd `
  -ConnectionString $connectionString `
  -InputFile ./CreateTable.sql

# az deployment group create `
#   --resource-group $ResourceGroup `
#   --template-file ./Application.bicep `
#   --parameters ./Application.bicepparam `
#   --output table
