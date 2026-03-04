#!/usr/bin/env pwsh
param(
    [string]$resourceGroupName = "rg-azure-serverless-web-api",
    [string]$location = "japaneast"
)

$ErrorActionPreference = "Stop"

# SQL パスワード
$sqlPassword = "Password123!"

# Resource Group の作成
az group create --name $resourceGroupName --location $location

# Bicep のデプロイ
az deployment group create `
    --resource-group $resourceGroupName `
    --template-file ./Main.bicep `
    --parameters sqlPassword=$sqlPassword `
    --query "properties.outputs" -o json
