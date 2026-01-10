// Params -------------------------------------------------------------------------------------------------------------
param serverName string
param publicIpAddress string
param sqlDBName string
param adminLogin string
@secure()
param adminLoginPassword string

// Variables ----------------------------------------------------------------------------------------------------------
var location = resourceGroup().location

// Resources ----------------------------------------------------------------------------------------------------------
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
resource firewallRulesAllowAzureServiceAccess 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = if (publicIpAddress != '*') {
  name: 'allow-access-azure-services'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}
resource firewallRulesAllowLocalPC 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = if (publicIpAddress != '*') {
  name: 'allow-access-local-pc'
  parent: sqlServer
  properties: {
    startIpAddress: publicIpAddress
    endIpAddress: publicIpAddress
  }
}
resource firewallRulesAllowAllDevice 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = if (publicIpAddress == '*') {
  name: 'allow-access-all-device'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '255.255.255.255'
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
// Outputs ------------------------------------------------------------------------------------------------------------
output connectionString string = 'Data Source=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};User ID=${adminLogin};Password=${adminLoginPassword};Encrypt=True;TrustServerCertificate=True;'
