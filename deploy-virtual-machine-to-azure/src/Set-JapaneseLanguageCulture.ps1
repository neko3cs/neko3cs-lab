#Requires -PSEdition Desktop

$GeoId = 122 # JAPAN
$TimeZone = "Tokyo Standard Time"

Set-WinCultureFromLanguageListOptOut `
  -OptOut $False
Set-WinHomeLocation `
  -GeoId $GeoId
Set-WinSystemLocale `
  -SystemLocale ja-JP
Set-WinUILanguageOverride `
  -Language ja-JP
Set-TimeZone `
  -Id $TimeZone
Restart-Computer
