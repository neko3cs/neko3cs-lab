// Params -------------------------------------------------------------------------------------------------------------
param location string = resourceGroup().location
param appName string
var useHttps = false // Httpsを使う場合はMEMOコメントの部分の設定をおこなう

// ResourceNames ------------------------------------------------------------------------------------------------------
var acaSubnetName = '${appName}-aca-subnet'
var agwSubnetName = '${appName}-agw-subnet'
var applicationGatewayName = '${appName}-agw'
var frontendHttpPortName = '${applicationGatewayName}-frontend-httpPort'
var frontendIPConfigName = '${applicationGatewayName}-frontendIpConfig'
var sslCertificatesName = '${applicationGatewayName}-sslCertificates'
var backendAddressPoolsName = '${applicationGatewayName}-backendAddressPools'
var backendHttpSettingsName = '${applicationGatewayName}-backendHttpSettings'
var httpListenerName = '${applicationGatewayName}-httpListeners'

// Resources ----------------------------------------------------------------------------------------------------------
resource virtualNetwork 'Microsoft.Network/virtualNetworks@2023-04-01' = {
  name: '${appName}-vnet'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '192.168.0.0/16'
      ]
    }
    subnets: [
      {
        name: acaSubnetName
        properties: {
          addressPrefix: '192.168.0.0/23'
        }
      }
      {
        name: agwSubnetName
        properties: {
          addressPrefix: '192.168.2.0/24'
        }
      }
    ]
  }
  resource acaSubnet 'subnets' existing = {
    name: acaSubnetName
  }
  resource agwSubnet 'subnets' existing = {
    name: agwSubnetName
  }
}
resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-11-02-preview' = {
  name: '${appName}-cae'
  location: location
  properties: {
    vnetConfiguration: {
      internal: true
      infrastructureSubnetId: virtualNetwork::acaSubnet.id
    }
  }
}
resource containerApp 'Microsoft.App/containerApps@2023-11-02-preview' = {
  name: '${appName}-aca'
  location: location
  properties: {
    configuration: {
      ingress: {
        external: true
        targetPort: 80
      }
    }
    environmentId: containerAppEnvironment.id
    template: {
      containers: [
        {
          name: 'nginx-container'
          image: 'nginx:latest'
        }
      ]
    }
  }
}
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: 'privatelink.${appName}-private-dns-zone'
  location: 'global'
}
resource privateDnsVNet 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  name: '${appName}-private-dns-zone-vnet'
  location: 'global'
  parent: privateDnsZone
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: virtualNetwork.id
    }
  }
}
resource containerAppDnsRecordApps 'Microsoft.Network/privateDnsZones/A@2020-06-01' = {
  name: '*'
  parent: privateDnsZone
  properties: {
    ttl: 3600
    aRecords: [
      {
        ipv4Address: containerAppEnvironment.properties.staticIp
      }
    ]
  }
}
resource publicIp 'Microsoft.Network/publicIPAddresses@2023-09-01' = {
  name: '${appName}-public-ip'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}
resource applicationGateway 'Microsoft.Network/applicationGateways@2023-09-01' = {
  name: applicationGatewayName
  location: location
  properties: {
    sku: {
      name: 'WAF_v2'
      tier: 'WAF_v2'
      capacity: 2
    }
    webApplicationFirewallConfiguration: {
      enabled: true
      firewallMode: 'Detection'
      ruleSetType: 'OWASP'
      ruleSetVersion: '3.2'
    }
    gatewayIPConfigurations: [
      {
        name: '${applicationGatewayName}-ipConfig'
        properties: {
          subnet: {
            id: virtualNetwork::agwSubnet.id
          }
        }
      }
    ]
    frontendPorts: [
      {
        name: frontendHttpPortName
        properties: {
          port: 80
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: frontendIPConfigName
        properties: {
          publicIPAddress: {
            id: publicIp.id
          }
        }
      }
    ]
    sslCertificates: useHttps
      ? [
          {
            name: sslCertificatesName
            properties: {
              // Httpsを使う場合は事前にCA証明書を取得して以下を設定する
              data: 'string' // loadFileAsBase64('filePath')
              keyVaultSecretId: 'string'
              password: 'string'
            }
          }
        ]
      : null
    backendAddressPools: [
      {
        name: backendAddressPoolsName
        properties: {
          backendAddresses: [
            {
              fqdn: containerApp.properties.configuration.ingress.fqdn
            }
          ]
        }
      }
    ]
    backendHttpSettingsCollection: [
      {
        name: backendHttpSettingsName
        properties: {
          cookieBasedAffinity: 'Disabled'
          port: useHttps ? 443 : 80
          protocol: useHttps ? 'Https' : 'Http'
          requestTimeout: 30
          pickHostNameFromBackendAddress: true
        }
      }
    ]
    httpListeners: [
      {
        name: httpListenerName
        properties: {
          frontendIPConfiguration: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/frontendIPConfigurations',
              applicationGatewayName,
              frontendIPConfigName
            )
          }
          frontendPort: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/frontendPorts',
              applicationGatewayName,
              frontendHttpPortName
            )
          }
          protocol: useHttps ? 'Https' : 'Http'
          sslCertificate: useHttps
            ? {
                id: resourceId(
                  'Microsoft.Network/applicationGateways/sslCertificates',
                  applicationGatewayName,
                  sslCertificatesName
                )
              }
            : null
        }
      }
    ]
    requestRoutingRules: [
      {
        name: '${applicationGatewayName}-requestRoutingRules'
        properties: {
          ruleType: 'Basic'
          priority: 100
          httpListener: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/httpListeners',
              applicationGatewayName,
              httpListenerName
            )
          }
          backendAddressPool: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/backendAddressPools',
              applicationGatewayName,
              backendAddressPoolsName
            )
          }
          backendHttpSettings: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/backendHttpSettingsCollection',
              applicationGatewayName,
              backendHttpSettingsName
            )
          }
        }
      }
    ]
  }
  dependsOn: [
    privateDnsVNet
  ]
}
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${appName}-logAnalyticsWorkspace'
  location: location
  properties: {
    sku: {
      name: 'Standalone'
    }
    retentionInDays: 7
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}
resource diagnpsticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: '${appName}-diagnpsticSettings'
  properties: {
    workspaceId: logAnalyticsWorkspace.id
    logs: [
      {
        category: 'Administrative'
        enabled: true
      }
      {
        category: 'Security'
        enabled: true
      }
      {
        category: 'ServiceHealth'
        enabled: true
      }
      {
        category: 'Alert'
        enabled: true
      }
      {
        category: 'Recommendation'
        enabled: true
      }
      {
        category: 'Policy'
        enabled: true
      }
      {
        category: 'Autoscale'
        enabled: true
      }
      {
        category: 'ResourceHealth'
        enabled: true
      }
    ]
  }
}
