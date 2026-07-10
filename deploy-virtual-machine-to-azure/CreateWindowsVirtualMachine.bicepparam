using './CreateWindowsVirtualMachine.bicep'

param adminUsername = 'azureuser'
param adminPassword = 'P@ssword!123'
param OSVersion = '2022-datacenter-g2'
param isDesktop = false
// param OSVersion = 'win11-25h2-pro'
// param isDesktop = true
param vmSize = 'Standard_D2s_v5'
param vmName = 'azurevm-dev-001'
param computerName = 'AZUREVM-DEV-001'
param diskSizeGB = 256
// 専用線(ExpressRoute/VPN)経由でRDP接続を許可するオンプレミス側のアドレス範囲(CIDR)に置き換えること
param onPremisesAddressPrefix = '203.0.113.0/24'
