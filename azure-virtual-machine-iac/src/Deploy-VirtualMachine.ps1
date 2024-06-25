#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$EnvVal = Get-Content -Path .env | ConvertFrom-StringData
$ResourceGroup = $EnvVal.RESOURCE_GROUP
$Location = $EnvVal.LOCATION
$AdminUserName = $EnvVal.ADMIN_USERNAME
$AdminPassword = $EnvVal.ADMIN_PASSWORD
$OSVersion = $EnvVal.OS_VERSION
$VMName = $EnvVal.VM_NAME
$VMSize = $EnvVal.VM_SIZE
$DiskSizeGB = $EnvVal.DISK_SIZE_GB
$AllowedIpAddress = (Invoke-RestMethod checkip.amazonaws.com)

az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./CreateWindowsServerVirtualMachine.bicep `
  --parameters `
  adminUsername="$AdminUserName" `
  adminPassword="$AdminPassword" `
  allowedIpAddress="$AllowedIpAddress" `
  vmName="$VMName" `
  OSVersion="$OSVersion" `
  vmName="$VMName" `
  vmSize="$VMSize" `
  diskSizeGB="$DiskSizeGB" `
  --output table

(az deployment group show `
  --resource-group $ResourceGroup `
  --name CreateWindowsServerVirtualMachine `
  --query properties.outputs.hostname.value).Trim('"') |
Out-File -FilePath hostname.txt -Encoding utf8 -NoNewline
