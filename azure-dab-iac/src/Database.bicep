// Params
param location string = resourceGroup().location
param serverName string
param publicIpAddress string
param sqlDBName string
param adminLogin string
@secure()
param adminLoginPassword string

// Resources
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminLoginPassword
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Enabled'
  }
}
resource firewallRulesAllowAzureServiceAccess 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = {
  name: 'allow-access-azure-services'
  parent: sqlServer
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
resource firewallRulesAllowLocalPC 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = {
  name: 'allow-access-local-pc'
  parent: sqlServer
  properties: {
    endIpAddress: publicIpAddress
    startIpAddress: publicIpAddress
  }
}
resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  name: sqlDBName
  parent: sqlServer
  location: location
  sku: {
    capacity: 1
    family: 'Gen5'
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
  }
  properties: {
    autoPauseDelay: 60
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    isLedgerOn: false
    minCapacity: 1
    readScale: 'Disabled'
    elasticPoolId: null
    requestedBackupStorageRedundancy: 'Local'
    zoneRedundant: false
  }
}
