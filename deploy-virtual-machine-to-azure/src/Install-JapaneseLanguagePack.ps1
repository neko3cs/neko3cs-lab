#Requires -PSEdition Desktop
$ErrorActionPreference = "Stop"

$OSCaption = (Get-CimInstance -ClassName Win32_OperatingSystem).Caption
$JapaneseKeyboardLayout = "0411:00000411"
$JapanGeoId = 122
$TimeZone = "Tokyo Standard Time"

if ($OSCaption -match "Microsoft Windows Server 2016") {
  lpksetup /i ja-JP /s
}
else {
  # After Windows Server 2016 OS
  Add-WindowsCapability -Online -Name Language.Basic~~~ja-JP~0.0.1.0 | Out-Null
  Add-WindowsCapability -Online -Name Language.Handwriting~~~ja-JP~0.0.1.0 | Out-Null
  Add-WindowsCapability -Online -Name Language.OCR~~~ja-JP~0.0.1.0 | Out-Null
  Add-WindowsCapability -Online -Name Language.Speech~~~ja-JP~0.0.1.0 | Out-Null
  Add-WindowsCapability -Online -Name Language.TextToSpeech~~~ja-JP~0.0.1.0 | Out-Null
  Add-WindowsCapability -Online -Name Language.Fonts.Japanese~~~ja-JP~0.0.1.0 | Out-Null
}

Set-WinUserLanguageList -LanguageList ja-JP, en-US -Force
Set-WinUILanguageOverride -Language ja-JP
Set-WinDefaultInputMethodOverride -InputTip $JapaneseKeyboardLayout
Set-WinCultureFromLanguageListOptOut -OptOut $False
Set-Culture -CultureInfo ja-JP
Set-WinHomeLocation -GeoId $JapanGeoId
Set-WinSystemLocale -SystemLocale ja-JP
Set-TimeZone -Id $TimeZone

Restart-Computer
