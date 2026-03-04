// --------------------------------------------------------------------------------
// メインオーケストレーター
// 各リソース（ネットワーク、データベース、アプリケーション等）のモジュールを呼び出し、
// システム全体のインフラを構築します。
// --------------------------------------------------------------------------------

param location string = resourceGroup().location
param projectName string = 'webapi'
param vnetAddressPrefix string = '10.0.0.0/16'
param funcSubnetPrefix string = '10.0.1.0/24'
param sqlSubnetPrefix string = '10.0.2.0/24'
param gatewaySubnetPrefix string = '10.0.3.0/24'

@secure()
param sqlPassword string

// リソース名が重複しないようにリソースグループのIDから一意の文字列を生成します。
var suffix = uniqueString(resourceGroup().id)

// 1. ネットワーク層の構築 (VNet, サブネット)
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

// 2. データベース層の構築 (SQL Server, SQL Database, Private Endpoint)
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

// 3. アプリケーション層の構築 (Functions, App Service Plan, Storage Account)
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

// 4. 公開層（ゲートウェイ）の構築 (Application Gateway + WAF)
// 最後に構築することで、Functions のホスト名を安全に受け取ることができます。
module appGateway './AppGateway.bicep' = {
  name: 'appGatewayDeployment'
  params: {
    location: location
    projectName: projectName
    appGatewaySubnetId: network.outputs.appGatewaySubnetId
    functionAppDefaultHostName: application.outputs.functionAppDefaultHostName
  }
}

// 他のツール（Deploy.ps1等）で利用するために必要な情報を出力します。
output sqlServerFqdn string = database.outputs.sqlServerFqdn
output sqlDbName string = database.outputs.sqlDbName
output sqlAdminLogin string = database.outputs.sqlAdminLogin
