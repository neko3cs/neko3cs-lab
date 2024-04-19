#Requires -PSEdition Desktop

$GeoId = 0x7A # JAPAN
$TimeZone = "Tokyo Standard Time"

Set-WinCultureFromLanguageListOptOut `
  -OptOut $False
Set-WinHomeLocation `
  -GeoId $GeoId
Set-WinSystemLocale `
  -SystemLocale ja-JP
Set-TimeZone `
  -Id $TimeZone
Restart-Computer
