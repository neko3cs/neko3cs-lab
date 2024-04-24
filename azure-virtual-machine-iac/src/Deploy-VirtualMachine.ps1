#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$EnvVal = Get-Content -Path .env | ConvertFrom-StringData
$ResourceGroup = $EnvVal.RESOURCE_GROUP
$Location = $EnvVal.LOCATION
$AdminUserName = $EnvVal.ADMIN_USERNAME
$AdminPassword = $EnvVal.ADMIN_PASSWORD
$AllowedIpAddress = $EnvVal.ALLOWED_IPADDRESS
$OSVersion = $EnvVal.OS_VERSION
$VMName = $EnvVal.VM_NAME
$VMSize = $EnvVal.VM_SIZE
$DiskSizeGB = $EnvVal.DISK_SIZE_GB

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
  OSVersion="$OSVersion" `
  vmName="$VMName" `
  vmSize="$VMSize" `
  diskSizeGB="$DiskSizeGB" `
  --output table

$HostName = az deployment group show `
  --resource-group $ResourceGroup `
  --name CreateWindowsServerVirtualMachine `
  --query properties.outputs.hostname.value `
  --output table

# TODO: 必要なスクリプトファイルをVMにアップロードし、実行する

if ($HostName) {
  Write-Output "HostName: '$HostName'"
}
