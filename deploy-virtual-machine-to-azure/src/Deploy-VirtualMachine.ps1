#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$Environ = Get-Content -Path .env | ConvertFrom-StringData
$ResourceGroup = $Environ.RESOURCE_GROUP
$Location = $Environ.LOCATION

$VirtualMachineParameters = @{
  "adminUsername" = @{ "value" = [string]$Environ.ADMIN_USERNAME };
  "adminPassword" = @{ "value" = [string]$Environ.ADMIN_PASSWORD };
  "OSVersion"     = @{ "value" = [string]$Environ.OS_VERSION };
  "vmSize"        = @{ "value" = [string]$Environ.VM_SIZE };
  "vmName"        = @{ "value" = [string]$Environ.VM_NAME };
  "computerName"  = @{ "value" = [string]$Environ.COMPUTER_NAME };
  "diskSizeGB"    = @{ "value" = [int]$Environ.DISK_SIZE_GB };
} |
ConvertTo-Json -Compress |
ForEach-Object { $_ -replace '"', '\"' }

az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

az deployment group create `
  --resource-group $ResourceGroup `
  --template-file ./CreateWindowsServerVirtualMachine.bicep `
  --parameters $VirtualMachineParameters `
  --output table
