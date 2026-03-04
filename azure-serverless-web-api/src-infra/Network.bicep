// --------------------------------------------------------------------------------
// ネットワーク・インフラ・モジュール
// VNet（仮想ネットワーク）と各リソース用のサブネットを定義します。
// --------------------------------------------------------------------------------

param location string
param projectName string
param vnetAddressPrefix string
param funcSubnetPrefix string
param sqlSubnetPrefix string
param gatewaySubnetPrefix string

// VNet (Virtual Network): システム全体が所属する仮想的なネットワーク空間です。
resource vnet 'Microsoft.Network/virtualNetworks@2023-11-01' = {
  name: '${projectName}-vnet'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressPrefix
      ]
    }
    subnets: [
      {
        // Azure Functions 用のサブネット
        name: 'FunctionSubnet'
        properties: {
          addressPrefix: funcSubnetPrefix
          delegations: [
            {
              // Azure Functions がこのサブネットを独占的に利用できるように設定します。
              name: 'flexDelegation'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
            }
          ]
        }
      }
      {
        // SQL Database (Private Endpoint) 用のサブネット
        name: 'SqlSubnet'
        properties: {
          addressPrefix: sqlSubnetPrefix
        }
      }
      {
        // Application Gateway 用のサブネット
        name: 'AppGatewaySubnet'
        properties: {
          addressPrefix: gatewaySubnetPrefix
        }
      }
    ]
  }
}

// 他のモジュールでリソースIDを利用するために出力します。
output vnetId string = vnet.id
output vnetName string = vnet.name
output functionSubnetId string = vnet.properties.subnets[0].id
output sqlSubnetId string = vnet.properties.subnets[1].id
output appGatewaySubnetId string = vnet.properties.subnets[2].id
