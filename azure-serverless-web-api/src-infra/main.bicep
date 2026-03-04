param location string = resourceGroup().location
param projectName string = 'webapi'
param vnetAddressPrefix string = '10.0.0.0/16'
param funcSubnetPrefix string = '10.0.1.0/24'
param sqlSubnetPrefix string = '10.0.2.0/24'
param gatewaySubnetPrefix string = '10.0.3.0/24'

@secure()
param sqlPassword string

var suffix = uniqueString(resourceGroup().id)

// 1. Network Module (VNet, Subnets)
module network './Network.bicep' = {
  name: 'networkDeployment'
  params: {
    location: location
    projectName: projectName
    vnetAddressPrefix: vnetAddressPrefix
    funcSubnetPrefix: funcSubnetPrefix
    sqlSubnetPrefix: sqlSubnetPrefix
    gatewaySubnetPrefix: gatewaySubnetPrefix
  }
}

// 2. Database Module (SQL Server, DB, Private Endpoint, Private DNS)
module database './Database.bicep' = {
  name: 'databaseDeployment'
  params: {
    location: location
    projectName: projectName
    suffix: suffix
    sqlPassword: sqlPassword
    vnetId: network.outputs.vnetId
    sqlSubnetId: network.outputs.sqlSubnetId
  }
}

// 3. Application Module (Storage, ASP, Function App)
module application './Application.bicep' = {
  name: 'applicationDeployment'
  params: {
    location: location
    projectName: projectName
    suffix: suffix
    functionSubnetId: network.outputs.functionSubnetId
    sqlServerFqdn: database.outputs.sqlServerFqdn
    sqlDbName: database.outputs.sqlDbName
    sqlAdminLogin: database.outputs.sqlAdminLogin
    sqlPassword: sqlPassword
  }
}

// 4. App Gateway Module (WAF, App Gateway) - Dependencies resolved
module appGateway './AppGateway.bicep' = {
  name: 'appGatewayDeployment'
  params: {
    location: location
    projectName: projectName
    appGatewaySubnetId: network.outputs.appGatewaySubnetId
    functionAppDefaultHostName: application.outputs.functionAppDefaultHostName
  }
}

output sqlServerFqdn string = database.outputs.sqlServerFqdn
output sqlDbName string = database.outputs.sqlDbName
output sqlAdminLogin string = database.outputs.sqlAdminLogin
