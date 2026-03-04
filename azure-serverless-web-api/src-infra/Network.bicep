param location string
param projectName string
param vnetAddressPrefix string
param funcSubnetPrefix string
param sqlSubnetPrefix string
param gatewaySubnetPrefix string

// VNet
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
        name: 'FunctionSubnet'
        properties: {
          addressPrefix: funcSubnetPrefix
          delegations: [
            {
              name: 'flexDelegation'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
            }
          ]
        }
      }
      {
        name: 'SqlSubnet'
        properties: {
          addressPrefix: sqlSubnetPrefix
        }
      }
      {
        name: 'AppGatewaySubnet'
        properties: {
          addressPrefix: gatewaySubnetPrefix
        }
      }
    ]
  }
}

output vnetId string = vnet.id
output vnetName string = vnet.name
output functionSubnetId string = vnet.properties.subnets[0].id
output sqlSubnetId string = vnet.properties.subnets[1].id
output appGatewaySubnetId string = vnet.properties.subnets[2].id
