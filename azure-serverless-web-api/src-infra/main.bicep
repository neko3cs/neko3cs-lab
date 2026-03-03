param location string = resourceGroup().location
param projectName string = 'az-srv-api'
param vnetAddressPrefix string = '10.0.0.0/16'
param funcSubnetPrefix string = '10.0.1.0/24'
param sqlSubnetPrefix string = '10.0.2.0/24'
param gatewaySubnetPrefix string = '10.0.3.0/24'

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
              name: 'serverfarmDelegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
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
        name: 'GatewaySubnet'
        properties: {
          addressPrefix: gatewaySubnetPrefix
        }
      }
    ]
  }
}

// Key Vault
resource kv 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: '${projectName}-kv'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
  }
}

// SQL Server & Database
resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: '${projectName}-sqlsvr'
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: 'Password123!' // 実際は Key Vault から取得すべき
    publicNetworkAccess: 'Enabled' // Azure サービスからのアクセスを許可するために有効化
  }
}

// 「Azure サービスおよびリソースにこのサーバーへのアクセスを許可する」をオンにする
resource allowAzureServices 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: '${projectName}-db'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

// Private Endpoint for SQL
resource sqlPrivateEndpoint 'Microsoft.Network/privateEndpoints@2023-11-01' = {
  name: '${projectName}-sql-pe'
  location: location
  properties: {
    subnet: {
      id: vnet.properties.subnets[1].id
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

// Private DNS Zone for SQL (閉域網での名前解決用)
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: 'privatelink.database.windows.net'
  location: 'global'
}

resource privateDnsZoneLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  parent: privateDnsZone
  name: '${projectName}-vnet-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnet.id
    }
  }
}

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

// Storage Account for Functions
resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${projectName}stg'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

// App Service Plan
resource asp 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: '${projectName}-asp'
  location: location
  sku: {
    name: 'EP1'
    tier: 'ElasticPremium'
  }
  kind: 'elastic'
}

// Azure Functions App
resource funcApp 'Microsoft.Web/sites@2023-12-01' = {
  name: '${projectName}-func'
  location: location
  kind: 'functionapp,linux'
  properties: {
    serverFarmId: asp.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};AccountKey=${storage.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'KEY_VAULT_URL'
          value: kv.properties.vaultUri
        }
      ]
    }
    virtualNetworkSubnetId: vnet.properties.subnets[0].id
  }
}

// Application Gateway + WAF
resource publicIp 'Microsoft.Network/publicIPAddresses@2023-11-01' = {
  name: '${projectName}-pip'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}

resource appGateway 'Microsoft.Network/applicationGateways@2023-11-01' = {
  name: '${projectName}-agw'
  location: location
  properties: {
    sku: {
      name: 'WAF_v2'
      tier: 'WAF_v2'
      capacity: 1
    }
    firewallPolicy: {
      id: wafPolicy.id
    }
    gatewayIPConfigurations: [
      {
        name: 'appGatewayIpConfig'
        properties: {
          subnet: {
            id: vnet.properties.subnets[2].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'appGatewayFrontendIp'
        properties: {
          publicIPAddress: {
            id: publicIp.id
          }
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port_80'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'funcBackendPool'
        properties: {
          backendAddresses: [
            {
              fqdn: funcApp.properties.defaultHostName
            }
          ]
        }
      }
    ]
    backendHttpSettingsCollection: [
      {
        name: 'funcHttpSettings'
        properties: {
          port: 443
          protocol: 'Https'
          cookieBasedAffinity: 'Disabled'
          pickHostNameFromBackendAddress: true
        }
      }
    ]
    httpListeners: [
      {
        name: 'appGatewayHttpListener'
        properties: {
          frontendIPConfiguration: {
            id: resourceId('Microsoft.Network/applicationGateways/frontendIPConfigurations', '${projectName}-agw', 'appGatewayFrontendIp')
          }
          frontendPort: {
            id: resourceId('Microsoft.Network/applicationGateways/frontendPorts', '${projectName}-agw', 'port_80')
          }
          protocol: 'Http'
        }
      }
    ]
    requestRoutingRules: [
      {
        name: 'rule1'
        properties: {
          ruleType: 'Basic'
          httpListener: {
            id: resourceId('Microsoft.Network/applicationGateways/httpListeners', '${projectName}-agw', 'appGatewayHttpListener')
          }
          backendAddressPool: {
            id: resourceId('Microsoft.Network/applicationGateways/backendAddressPools', '${projectName}-agw', 'funcBackendPool')
          }
          backendHttpSettings: {
            id: resourceId('Microsoft.Network/applicationGateways/backendHttpSettingsCollection', '${projectName}-agw', 'funcHttpSettings')
          }
          priority: 1
        }
      }
    ]
  }
}

resource wafPolicy 'Microsoft.Network/ApplicationGatewayWebApplicationFirewallPolicies@2023-11-01' = {
  name: '${projectName}-wafpolicy'
  location: location
  properties: {
    policySettings: {
      state: 'Enabled'
      mode: 'Prevention'
    }
    managedRules: {
      managedRuleSets: [
        {
          ruleSetType: 'OWASP'
          ruleSetVersion: '3.2'
        }
      ]
    }
  }
}

output keyVaultName string = kv.name
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output sqlDbName string = sqlDb.name
output sqlAdminLogin string = sqlServer.properties.administratorLogin
