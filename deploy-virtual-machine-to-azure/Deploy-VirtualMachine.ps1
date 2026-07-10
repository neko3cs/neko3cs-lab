#!/usr/bin/env pwsh
#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$ResourceGroup = "rg-azurevm-dev-japaneast-001"
$Location = "japaneast"

az group create --name $ResourceGroup --location $Location --output table

az deployment group create `
  --resource-group $ResourceGroup `
  --parameters ./CreateWindowsVirtualMachine.bicepparam `
  --output table
