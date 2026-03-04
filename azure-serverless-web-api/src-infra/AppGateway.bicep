// --------------------------------------------------------------------------------
// 公開層（ゲートウェイ）モジュール
// Application Gateway と WAF ポリシーを定義し、外部からのアクセスを制御します。
// --------------------------------------------------------------------------------

param location string
param projectName string
param appGatewaySubnetId string
param functionAppDefaultHostName string

// Public IP: Application Gateway がインターネットから通信を受け取るための固定IPです。
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

// WAF (Web Application Firewall) Policy: 
// 悪意のある攻撃（SQLインジェクションなど）から API を保護するルールセットです。
resource wafPolicy 'Microsoft.Network/ApplicationGatewayWebApplicationFirewallPolicies@2023-11-01' = {
  name: '${projectName}-wafpolicy'
  location: location
  properties: {
    policySettings: {
      state: 'Enabled'
      mode: 'Prevention' // 攻撃を検知した際にブロックするモードです。
    }
    managedRules: {
      managedRuleSets: [
        {
          // 標準的な攻撃パターンを網羅した OWASP ルールセットを使用します。
          ruleSetType: 'OWASP'
          ruleSetVersion: '3.2'
        }
      ]
    }
  }
}

// Application Gateway: 
// WAF を備えたロードバランサーとして機能し、Functions へリクエストを中継します。
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
            id: appGatewaySubnetId
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
        // バックエンドとして Azure Functions のホスト名を指定します。
        name: 'funcBackendPool'
        properties: {
          backendAddresses: [
            {
              fqdn: functionAppDefaultHostName
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
          pickHostNameFromBackendAddress: true // バックエンドのホスト名を自動解決します。
        }
      }
    ]
    httpListeners: [
      {
        name: 'appGatewayHttpListener'
        properties: {
          frontendIPConfiguration: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/frontendIPConfigurations',
              '${projectName}-agw',
              'appGatewayFrontendIp'
            )
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
        // 受け取ったリクエストを Functions のバックエンドプールに振り分けます。
        name: 'rule1'
        properties: {
          ruleType: 'Basic'
          httpListener: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/httpListeners',
              '${projectName}-agw',
              'appGatewayHttpListener'
            )
          }
          backendAddressPool: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/backendAddressPools',
              '${projectName}-agw',
              'funcBackendPool'
            )
          }
          backendHttpSettings: {
            id: resourceId(
              'Microsoft.Network/applicationGateways/backendHttpSettingsCollection',
              '${projectName}-agw',
              'funcHttpSettings'
            )
          }
          priority: 1
        }
      }
    ]
  }
}
