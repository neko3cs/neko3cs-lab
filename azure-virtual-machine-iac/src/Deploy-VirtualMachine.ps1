#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$EnvVal = Get-Content -Path .env | ConvertFrom-StringData
$ResourceGroup = $EnvVal.RESOURCE_GROUP
$Location = $EnvVal.LOCATION
$AdminUserName = $EnvVal.ADMIN_USERNAME
$AdminPassword = $EnvVal.ADMIN_PASSWORD
$OSVersion = $EnvVal.OS_VERSION
$VMName = $EnvVal.VM_NAME

az group create `
  --name $ResourceGroup `
  --location $Location

az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./CreateWindowsServerVirtualMachine.bicep `
  --parameters `
  adminUsername="$AdminUserName" `
  adminPassword="$AdminPassword" `
  OSVersion="$OSVersion" `
  vmName="$VMName"

$HostName = az deployment group show `
  --resource-group $ResourceGroup `
  --name CreateWindowsServerVirtualMachine `
  --query properties.outputs.hostname.value

Write-Output "HostName: '$HostName'"
