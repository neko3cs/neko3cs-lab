#Requires -PSEdition Desktop

Get-NetFirewallRule `
  -Name FPS-ICMP4-ERQ-In |
Set-NetFirewallRule `
  -Enabled true
Get-NetFirewallRule `
  -Name FPS-ICMP6-ERQ-In |
Set-NetFirewallRule `
  -Enabled true
