// --------------------------------------------------------------------------------
// アプリケーション・モジュール
// Azure Functions、App Service Plan、Storage Account を定義します。
// --------------------------------------------------------------------------------

param location string
param projectName string
param suffix string
param functionSubnetId string
param sqlServerFqdn string
param sqlDbName string
param sqlAdminLogin string
@secure()
param sqlPassword string

// リソース名の定義
var storageAccountName = take('stg${projectName}${suffix}', 24)

// Storage Account: Azure Functions が動作するために必要なストレージです。
resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

// Blob Container: デプロイパッケージを置くためのコンテナです。
resource deploymentContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  name: '${storage.name}/default/deployment'
}

// App Service Plan (Flex Consumption): 
// サーバーレスでありながら VNet 統合（閉域網接続）が可能な最新のプランです。
resource asp 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: '${projectName}-asp'
  location: location
  sku: {
    name: 'FC1'
    tier: 'FlexConsumption'
  }
  kind: 'functionapp'
  properties: {
    reserved: true // Linux プランの場合に必要です。
  }
}

// Azure Functions App: 実際の API プログラムが動作するリソースです。
resource funcApp 'Microsoft.Web/sites@2023-12-01' = {
  name: '${projectName}-func'
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned' // マネージドID（リソース自身の身分証明書）を有効にします。
  }
  properties: {
    serverFarmId: asp.id
    functionAppConfig: {
      runtime: {
        name: 'dotnet-isolated'
        version: '10.0'
      }
      // Flex Consumption プラン固有のデプロイ設定
      deployment: {
        storage: {
          type: 'blobContainer'
          value: '${storage.properties.primaryEndpoints.blob}deployment'
          authentication: {
            type: 'StorageAccountConnectionString'
            storageAccountConnectionStringName: 'DEPLOYMENT_STORAGE_CONNECTION_STRING'
          }
        }
      }
      scaleAndConcurrency: {
        instanceMemoryMB: 2048
        maximumInstanceCount: 100
      }
    }
    siteConfig: {
      // 環境変数（App Settings）の設定
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};AccountKey=${storage.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
        }
        {
          name: 'DEPLOYMENT_STORAGE_CONNECTION_STRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};AccountKey=${storage.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
        }
        {
          // データベース接続文字列を環境変数として渡します。
          name: 'DbConnectionString'
          value: 'Server=tcp:${sqlServerFqdn},1433;Initial Catalog=${sqlDbName};User ID=${sqlAdminLogin};Password=${sqlPassword};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
        }
        {
          // ミドルウェアで使用する API キーです。
          name: 'WebApiApiKey'
          value: 'DefaultApiKey123'
        }
      ]
    }
    // VNet 統合の設定: この Functions を VNet 内のサブネットに参加させます。
    virtualNetworkSubnetId: functionSubnetId
  }
}

// Application Gateway で使用するために、ホスト名を出力します。
output functionAppDefaultHostName string = funcApp.properties.defaultHostName
