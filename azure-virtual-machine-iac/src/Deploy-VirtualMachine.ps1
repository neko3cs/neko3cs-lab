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
$initializeScripts = @(
  'Enable-PingFirewallRule.ps1'
  'Install-JapaneseLanguagePack.ps1'
  'Set-JapaneseLanguageCulture.ps1'
)

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

$hostName = Get-Content hostname.txt
ssh-keygen -R $hostName
az vm run-command invoke `
  --resource-group $ResourceGroup `
  --name $VMName `
  --command-id RunPowerShellScript `
  --scripts 'mkdir C:\Setup\'
foreach ($script in $initializeScripts) {
  scp "./$script" "$($AdminUserName)@$($hostName):/Setup/" # HACK: scpコマンドではパスワードが対話形式で聞かれてしまうので回避策を考える
  $status = 'Running'
  while ($status -ne 'Succeeded') {
    $status = (az vm get-instance-view `
        --resource-group $ResourceGroup `
        --name $VMName `
        --query instanceView.statuses[1].displayStatus `
        --output tsv).Trim('"')
    if ($status -ne 'VM running') {
      Write-Output 'Wait for VM to restart...'
      Start-Sleep -Seconds 10
    }
    else {
      $status = 'Succeeded'
    }
  }
  az vm run-command invoke `
    --resource-group $ResourceGroup `
    --name $VMName `
    --command-id RunPowerShellScript `
    --scripts ". C:\Setup\$script" # FIXME: 'Install-JapaneseLanguagePack.ps1'が乙る
}
