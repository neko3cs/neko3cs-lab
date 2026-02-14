// Params -------------------------------------------------------------------------------------------------------------
param appName string
@secure()
param connectionString string
param storageAccountName string
@secure()
param storageAccountKey string
param dabConfigFileName string = 'dab-config.json'
param mountedStorageName string = 'dabconfig'

// ResourceNames ------------------------------------------------------------------------------------------------------
var useHttps = false // Httpsを使う場合はMEMOコメントの部分の設定をおこなう
var location = resourceGroup().location
var acaSubnetName = '${appName}-aca-subnet'
var agwSubnetName = '${appName}-agw-subnet'
var applicationGatewayName = '${appName}-agw'
var frontendHttpPortName = '${applicationGatewayName}-frontend-httpPort'
var frontendIPConfigName = '${applicationGatewayName}-frontendIpConfig'
var sslCertificatesName = '${applicationGatewayName}-sslCertificates'
var backendAddressPoolsName = '${applicationGatewayName}-backendAddressPools'
var backendHttpSettingsName = '${applicationGatewayName}-backendHttpSettings'
var httpListenerName = '${applicationGatewayName}-httpListeners'
var dabConfigFilePath = '--ConfigFileName=/${mountedStorageName}/${dabConfigFileName}'

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
          delegations: [
            {
              name: 'delegation'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
            }
          ]
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
resource publicIp 'Microsoft.Network/publicIPAddresses@2023-09-01' = {
  name: '${appName}-public-ip'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    dnsSettings: {
      domainNameLabel: '${appName}-${uniqueString(resourceGroup().id)}'
    }
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
          port: 80
          protocol: 'Http'
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
}
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${appName}-logAnalyticsWorkspace'
  location: location
  properties: {
    retentionInDays: 30
    sku: {
      name: 'Standalone'
    }
  }
}
resource diagnosticSettingsAgw 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: '${appName}-agw-diagnostic'
  scope: applicationGateway
  properties: {
    workspaceId: logAnalyticsWorkspace.id
    logs: [
      {
        category: 'ApplicationGatewayAccessLog'
        enabled: true
      }
      {
        category: 'ApplicationGatewayPerformanceLog'
        enabled: true
      }
      {
        category: 'ApplicationGatewayFirewallLog'
        enabled: true
      }
    ]
  }
}
resource diagnosticSettingsCae 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: '${appName}-cae-diagnostic'
  scope: containerAppEnvironment
  properties: {
    workspaceId: logAnalyticsWorkspace.id
    logs: [
      {
        category: 'ContainerAppConsoleLogs'
        enabled: true
      }
    ]
  }
}
resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-11-02-preview' = {
  name: '${appName}-cae'
  location: location
  properties: {
    workloadProfiles: [
      {
        name: 'Consumption'
        workloadProfileType: 'Consumption'
      }
    ]
    vnetConfiguration: {
      internal: false
      infrastructureSubnetId: virtualNetwork::acaSubnet.id
    }
  }
}

resource environmentStorage 'Microsoft.App/managedEnvironments/storages@2023-11-02-preview' = {
  parent: containerAppEnvironment
  name: mountedStorageName
  properties: {
    azureFile: {
      accountName: storageAccountName
      accountKey: storageAccountKey
      shareName: mountedStorageName
      accessMode: 'ReadWrite'
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
        targetPort: 5000
        allowInsecure: true
      }
    }
    environmentId: containerAppEnvironment.id
    workloadProfileName: 'Consumption'
    template: {
      containers: [
        {
          name: 'dab-container'
          image: 'mcr.microsoft.com/azure-databases/data-api-builder:latest'
          env: [
            {
              name: 'DATABASE_CONNECTION_STRING'
              value: connectionString
            }
          ]
          args: [
            dabConfigFilePath
          ]
          volumeMounts: [
            {
              volumeName: 'azure-file-volume'
              mountPath: '/${mountedStorageName}'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 10
      }
      volumes: [
        {
          name: 'azure-file-volume'
          storageType: 'AzureFile'
          storageName: mountedStorageName
        }
      ]
    }
  }
  dependsOn: [
    environmentStorage
  ]
}

output dabUrl string = 'http://${publicIp.properties.dnsSettings.fqdn}'
output containerAppFqdn string = containerApp.properties.configuration.ingress.fqdn
