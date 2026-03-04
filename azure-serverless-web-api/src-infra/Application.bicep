param location string
param projectName string
param suffix string
param functionSubnetId string
param sqlServerFqdn string
param sqlDbName string
param sqlAdminLogin string
@secure()
param sqlPassword string

var storageAccountName = take('stg${projectName}${suffix}', 24)

// Storage Account for Functions
resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

// デプロイ用コンテナ
resource deploymentContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  name: '${storage.name}/default/deployment'
}

// App Service Plan
resource asp 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: '${projectName}-asp'
  location: location
  sku: {
    name: 'FC1'
    tier: 'FlexConsumption'
  }
  kind: 'functionapp'
  properties: {
    reserved: true
  }
}

// Azure Functions App
resource funcApp 'Microsoft.Web/sites@2023-12-01' = {
  name: '${projectName}-func'
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: asp.id
    functionAppConfig: {
      runtime: {
        name: 'dotnet-isolated'
        version: '10.0'
      }
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
          name: 'DbConnectionString'
          value: 'Server=tcp:${sqlServerFqdn},1433;Initial Catalog=${sqlDbName};User ID=${sqlAdminLogin};Password=${sqlPassword};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
        }
        {
          name: 'WebApiApiKey'
          value: 'DefaultApiKey123'
        }
      ]
    }
    virtualNetworkSubnetId: functionSubnetId
  }
}

output functionAppDefaultHostName string = funcApp.properties.defaultHostName
