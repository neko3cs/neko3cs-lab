#Requires -PSEdition Desktop
$ErrorActionPreference = "Stop"

$OSCaption = (Get-CimInstance -ClassName Win32_OperatingSystem).Caption
$JapaneseKeyboardLayout = "0411:00000411"
$JapanGeoId = 122
$TimeZone = "Tokyo Standard Time"

if ($OSCaption -match "Microsoft Windows Server 2016") {
  lpksetup /i ja-JP /s

  Set-WinUserLanguageList `
    -LanguageList ja-JP, en-US `
    -Force
  Set-WinUILanguageOverride `
    -Language ja-JP
}
else {
  # After Windows Server 2016 OS
  Install-Module `
    -Name LanguagePackManagement `
    -Force -AllowClobber
  Add-WindowsCapability `
    -Online `
    -Name Language.Basic~~~ja-JP~0.0.1.0
  Add-WindowsCapability `
    -Online `
    -Name Language.Fonts.Japanese~~~ja-JP~0.0.1.0
  Add-WindowsCapability `
    -Online `
    -Name Language.TextToSpeech~~~ja-JP~0.0.1.0

  Set-SystemPreferredUILanguage `
    -Language ja-JP
}

Set-WinDefaultInputMethodOverride `
  -InputTip $JapaneseKeyboardLayout
Set-WinCultureFromLanguageListOptOut `
  -OptOut $False
Set-WinHomeLocation `
  -GeoId $JapanGeoId
Set-WinSystemLocale `
  -SystemLocale ja-JP
Set-TimeZone `
  -Id $TimeZone

Restart-Computer
