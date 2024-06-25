#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

$EnvVal = Get-Content -Path .env | ConvertFrom-StringData
$ResourceGroup = $EnvVal.RESOURCE_GROUP
$Location = $EnvVal.LOCATION

$VirtualMachineParameters = @{
  "adminUsername"    = @{ "value" = [string]$EnvVal.ADMIN_USERNAME };
  "adminPassword"    = @{ "value" = [string]$EnvVal.ADMIN_PASSWORD };
  "allowedIpAddress" = @{ "value" = [string]((Invoke-RestMethod checkip.amazonaws.com).Trim()) };
  "OSVersion"        = @{ "value" = [string]$EnvVal.OS_VERSION };
  "vmName"           = @{ "value" = [string]$EnvVal.VM_NAME };
  "vmSize"           = @{ "value" = [string]$EnvVal.VM_SIZE };
  "diskSizeGB"       = @{ "value" = [int]$EnvVal.DISK_SIZE_GB };
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

(az deployment group show `
  --resource-group $ResourceGroup `
  --name CreateWindowsServerVirtualMachine `
  --query properties.outputs.hostname.value).Trim('"') |
Out-File -FilePath hostname.txt -Encoding utf8 -NoNewline
