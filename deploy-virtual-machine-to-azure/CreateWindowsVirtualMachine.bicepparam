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
param deployNetworkSecurityGroup = true
