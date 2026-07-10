#!/usr/bin/env pwsh
#Requires -PSEdition Core

param (
  [string]$ResourceGroup = "rg-azurevm-dev-japaneast-001",
  [string]$Location = "japaneast"
)

$ErrorActionPreference = 'Stop'

az group create --name $ResourceGroup --location $Location --output table

az deployment group create `
  --resource-group $ResourceGroup `
  --parameters ./CreateWindowsVirtualMachine.bicepparam `
  --output table
