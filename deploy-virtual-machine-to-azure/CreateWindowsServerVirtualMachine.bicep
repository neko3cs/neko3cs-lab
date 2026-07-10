// Parameter ----------------------------------------------------------------------------------------------------------
@description('VMにサインインする管理者ユーザー名。administrator等、Azureの予約語は使用不可。')
@minLength(1)
@maxLength(20)
param adminUsername string
@description('VMにサインインする管理者パスワード。12文字以上で、大文字・小文字・数字・記号のうち3種類以上を含める必要がある。')
@minLength(12)
@secure()
param adminPassword string
@description('デプロイするOSイメージのSKUバージョン。')
param OSVersion string
@description('trueの場合はWindows 11 Desktopイメージ、falseの場合はWindows Serverイメージを使用する。')
param isDesktop bool = false
@description('VMのサイズ(SKU)。')
param vmSize string
@description('VMリソース名。')
param vmName string
@description('VMのコンピューター名(NetBIOS名。15文字以内)。')
@maxLength(15)
param computerName string
@description('OSディスクのサイズ(GB)。Windowsイメージが要求する最小サイズ以上を指定する。')
@minValue(127)
@maxValue(2048)
param diskSizeGB int
@description('ExpressRoute/VPN等の専用線経由でVMに直接RDP接続することを許可するオンプレミス側のアドレス範囲(CIDR)。')
param onPremisesAddressPrefix string

// Variables ----------------------------------------------------------------------------------------------------------
var location = resourceGroup().location
var bootDiagStorageAccountName = 'bootdiags${uniqueString(resourceGroup().id)}'
var nicName = '${vmName}-VMNic'
var addressPrefix = '10.0.0.0/16'
var subnetName = 'Subnet'
var subnetPrefix = '10.0.0.0/24'
var bastionName = '${vmName}-Bastion'
var bastionSubnetName = 'AzureBastionSubnet'
var bastionAddressPrefix = '10.0.1.0/24'
var virtualNetworkName = '${vmName}-VNET'
var bastionPublicIpName = '${vmName}-BastionPublicIP'
var bastionPublicIPAllocationMethod = 'Static'
var bastionPublicIpSku = 'Standard'
var subnetNsgName = '${vmName}-Subnet-nsg'
var bastionNsgName = '${bastionName}-nsg'
var securityType = 'TrustedLaunch'
var securityProfileJson = {
  uefiSettings: {
    secureBootEnabled: true
    vTpmEnabled: true
  }
  securityType: securityType
}
var extensionName = 'GuestAttestation'
var extensionPublisher = 'Microsoft.Azure.Security.WindowsAttestation'
var extensionVersion = '1.0'
var maaTenantName = 'GuestAttestation'
var maaEndpoint = ''

// Resources ----------------------------------------------------------------------------------------------------------
resource bootDiagStorageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: bootDiagStorageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
    allowBlobPublicAccess: false
  }
}
resource subnetNsg 'Microsoft.Network/networkSecurityGroups@2023-09-01' = {
  name: subnetNsgName
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowRdpFromBastion'
        properties: {
          priority: 100
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '3389'
          sourceAddressPrefix: bastionAddressPrefix
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'AllowRdpFromOnPremises'
        properties: {
          priority: 110
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '3389'
          sourceAddressPrefix: onPremisesAddressPrefix
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'DenyAllInbound'
        properties: {
          priority: 4096
          direction: 'Inbound'
          access: 'Deny'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}
resource bastionNsg 'Microsoft.Network/networkSecurityGroups@2023-09-01' = {
  name: bastionNsgName
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowHttpsInbound'
        properties: {
          priority: 120
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: 'Internet'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'AllowGatewayManagerInbound'
        properties: {
          priority: 130
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: 'GatewayManager'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'AllowAzureLoadBalancerInbound'
        properties: {
          priority: 140
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: 'AzureLoadBalancer'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'AllowBastionHostCommunicationInbound'
        properties: {
          priority: 150
          direction: 'Inbound'
          access: 'Allow'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '8080'
            '5701'
          ]
          sourceAddressPrefix: 'VirtualNetwork'
          destinationAddressPrefix: 'VirtualNetwork'
        }
      }
      {
        name: 'DenyAllInbound'
        properties: {
          priority: 4096
          direction: 'Inbound'
          access: 'Deny'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'AllowSshRdpOutbound'
        properties: {
          priority: 100
          direction: 'Outbound'
          access: 'Allow'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '22'
            '3389'
          ]
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'VirtualNetwork'
        }
      }
      {
        name: 'AllowAzureCloudOutbound'
        properties: {
          priority: 110
          direction: 'Outbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'AzureCloud'
        }
      }
      {
        name: 'AllowBastionCommunicationOutbound'
        properties: {
          priority: 120
          direction: 'Outbound'
          access: 'Allow'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '8080'
            '5701'
          ]
          sourceAddressPrefix: 'VirtualNetwork'
          destinationAddressPrefix: 'VirtualNetwork'
        }
      }
      {
        name: 'AllowGetSessionInformationOutbound'
        properties: {
          priority: 130
          direction: 'Outbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '80'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'Internet'
        }
      }
      {
        name: 'DenyAllOutbound'
        properties: {
          priority: 4096
          direction: 'Outbound'
          access: 'Deny'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}
resource bastionPublicIp 'Microsoft.Network/publicIPAddresses@2023-09-01' = {
  name: bastionPublicIpName
  location: location
  sku: {
    name: bastionPublicIpSku
  }
  properties: {
    publicIPAllocationMethod: bastionPublicIPAllocationMethod
  }
}
resource virtualNetwork 'Microsoft.Network/virtualNetworks@2023-09-01' = {
  name: virtualNetworkName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressPrefix
      ]
    }
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: subnetPrefix
          networkSecurityGroup: {
            id: subnetNsg.id
          }
        }
      }
      {
        name: bastionSubnetName
        properties: {
          addressPrefix: bastionAddressPrefix
          networkSecurityGroup: {
            id: bastionNsg.id
          }
        }
      }
    ]
  }
}
resource nic 'Microsoft.Network/networkInterfaces@2023-09-01' = {
  name: nicName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          subnet: {
            id: virtualNetwork.properties.subnets[0].id
          }
        }
      }
    ]
  }
}
resource vm 'Microsoft.Compute/virtualMachines@2023-09-01' = {
  name: vmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: vmSize
    }
    osProfile: {
      computerName: computerName
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: {
        provisionVMAgent: true
      }
    }
    storageProfile: {
      imageReference: isDesktop
        ? {
            publisher: 'MicrosoftWindowsDesktop'
            offer: 'Windows-11'
            sku: OSVersion
            version: 'latest'
          }
        : {
            publisher: 'MicrosoftWindowsServer'
            offer: 'WindowsServer'
            sku: OSVersion
            version: 'latest'
          }
      osDisk: {
        createOption: 'FromImage'
        managedDisk: {
          storageAccountType: 'StandardSSD_LRS'
        }
        deleteOption: 'Delete'
        diskSizeGB: diskSizeGB
      }
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nic.id
          properties: {
            deleteOption: 'Delete'
          }
        }
      ]
    }
    diagnosticsProfile: {
      bootDiagnostics: {
        enabled: true
        storageUri: bootDiagStorageAccount.properties.primaryEndpoints.blob
      }
    }
    securityProfile: ((securityType == 'TrustedLaunch') ? securityProfileJson : null)
  }
}
resource vmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-09-01' = if (securityType == 'TrustedLaunch') {
  parent: vm
  name: extensionName
  location: location
  properties: {
    publisher: extensionPublisher
    type: extensionName
    typeHandlerVersion: extensionVersion
    autoUpgradeMinorVersion: true
    enableAutomaticUpgrade: true
    settings: {
      AttestationConfig: {
        MaaSettings: {
          maaEndpoint: maaEndpoint
          maaTenantName: maaTenantName
        }
      }
    }
  }
}
resource bastion 'Microsoft.Network/bastionHosts@2023-09-01' = {
  name: bastionName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: '${bastionName}-ipConfig'
        properties: {
          subnet: {
            id: virtualNetwork.properties.subnets[1].id
          }
          publicIPAddress: {
            id: bastionPublicIp.id
          }
        }
      }
    ]
  }
}

// Outputs --------------------------------------------------------------------------------------------------------
@description('デプロイしたVMのリソースID。')
output vmResourceId string = vm.id
@description('VMのプライベートIPアドレス。')
output vmPrivateIpAddress string = nic.properties.ipConfigurations[0].properties.privateIPAddress
@description('BastionのパブリックIPアドレス。')
output bastionPublicIpAddress string = bastionPublicIp.properties.ipAddress
