// --------------------------------------------------------------------------------
// データベース・モジュール
// Azure SQL Database と、その閉域網アクセス (Private Endpoint) を定義します。
// --------------------------------------------------------------------------------

param location string
param projectName string
param suffix string
@secure()
param sqlPassword string
param vnetId string
param sqlSubnetId string

// リソース名の定義
var sqlServerName = take('sql-${projectName}-${suffix}', 24)

// SQL Server: データベースの入れ物（サーバー）を作成します。
resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: sqlPassword
    publicNetworkAccess: 'Enabled' // インターネットからのアクセス。本来はセキュリティ上Disabledが理想。
  }
}

// ファイアウォールルール: 他の Azure サービス（Functionsなど）からのアクセスを許可します。
resource allowAzureServices 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// SQL Database: 実際のデータを保持するデータベースを作成します。
resource sqlDb 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: '${projectName}db'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

// Private Endpoint: SQL サーバーに VNet 内のプライベートIPを割り当て、閉域網からアクセス可能にします。
resource sqlPrivateEndpoint 'Microsoft.Network/privateEndpoints@2023-11-01' = {
  name: '${projectName}-sql-pe-${suffix}'
  location: location
  properties: {
    subnet: {
      id: sqlSubnetId
    }
    privateLinkServiceConnections: [
      {
        name: 'sqlConnection'
        properties: {
          privateLinkServiceId: sqlServer.id
          groupIds: [
            'sqlServer'
          ]
        }
      }
    ]
  }
}

// Private DNS Zone: VNet 内で SQL サーバーの名前をプライベートIPに解決できるようにします。
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: 'privatelink${environment().suffixes.sqlServerHostname}'
  location: 'global'
}

// DNS Zone と VNet を紐づけます。
resource privateDnsZoneLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  parent: privateDnsZone
  name: '${projectName}-vnet-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}

// Private Endpoint と DNS Zone を紐づけ、自動的に A レコードを作成します。
resource privateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2023-11-01' = {
  parent: sqlPrivateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'sqlConfig'
        properties: {
          privateDnsZoneId: privateDnsZone.id
        }
      }
    ]
  }
}

// 接続文字列の作成に必要な情報を出力します。
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output sqlDbName string = sqlDb.name
output sqlAdminLogin string = sqlServer.properties.administratorLogin
