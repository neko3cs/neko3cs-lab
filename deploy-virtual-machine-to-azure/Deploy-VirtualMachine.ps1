#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$ResourceGroup = "rg-azurevm-dev-japaneast-001"
$Location = "japaneast"
$AdminUsername = "azureuser"
$AdminPassword = "P@ssword!123"
$OSVersion = "2022-datacenter-g2"
$IsDesktop = $false
# $OSVersion = "win11-25h2-pro"
# $IsDesktop = $true
$VMSize = "Standard_D2s_v5"
$VMName = "azurevm-dev-001"
$ComputerName = "AZUREVM-DEV-001"
$DiskSizeGB = 256

az group create --name $ResourceGroup --location $Location --output table

az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./CreateWindowsServerVirtualMachine.bicep `
  --parameters `
  adminUsername=$AdminUsername `
  adminPassword=$AdminPassword `
  OSVersion=$OSVersion `
  isDesktop=$IsDesktop `
  vmSize=$VMSize `
  vmName=$VMName `
  computerName=$ComputerName `
  diskSizeGB=$DiskSizeGB `
  --output table
