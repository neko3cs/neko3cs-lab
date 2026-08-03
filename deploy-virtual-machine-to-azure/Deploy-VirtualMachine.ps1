#!/usr/bin/env pwsh
#Requires -PSEdition Core

param (
  [string]$SubscriptionName,
  [string]$ResourceGroup = "rg-azurevm-dev-japaneast-001",
  [string]$Location = "japaneast"
)

$ErrorActionPreference = 'Stop'

# $SubscriptionName 未指定時はAzure CLIの既定サブスクリプションを使用する。
$SubscriptionArgs = if ($SubscriptionName) { @('--subscription', $SubscriptionName) } else { @() }

$TargetSubscription = az account show @SubscriptionArgs --query name --output tsv
if ($LASTEXITCODE -ne 0) {
  throw "サブスクリプションの取得に失敗しました。az login の状態と SubscriptionName の指定を確認してください。"
}
Write-Host "デプロイ先サブスクリプション: $TargetSubscription"

$IsResourceGroupExists = az group exists --name $ResourceGroup @SubscriptionArgs --output tsv
if ($LASTEXITCODE -ne 0) {
  throw "リソースグループの存在確認に失敗しました: $ResourceGroup"
}

if ($IsResourceGroupExists -eq 'true') {
  Write-Host "リソースグループ $ResourceGroup は既に存在するため作成をスキップします。"
}
else {
  Write-Host "リソースグループ $ResourceGroup を作成します。"
  az group create --name $ResourceGroup --location $Location @SubscriptionArgs --output table
  if ($LASTEXITCODE -ne 0) {
    throw "リソースグループの作成に失敗しました: $ResourceGroup"
  }
}

az deployment group create `
  --resource-group $ResourceGroup `
  --parameters ./CreateWindowsVirtualMachine.bicepparam `
  @SubscriptionArgs `
  --output table
if ($LASTEXITCODE -ne 0) {
  throw "デプロイに失敗しました。"
}
