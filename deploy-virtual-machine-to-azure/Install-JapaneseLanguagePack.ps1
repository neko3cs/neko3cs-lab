#Requires -PSEdition Desktop
$ErrorActionPreference = "Stop"

$Capabilities = @(
  "Language.Basic~~~ja-JP~0.0.1.0"
  "Language.Fonts.Jpan~~~und-JPAN~0.0.1.0"
  "Language.Handwriting~~~ja-JP~0.0.1.0"
  "Language.OCR~~~ja-JP~0.0.1.0"
  "Language.Speech~~~ja-JP~0.0.1.0"
  "Language.TextToSpeech~~~ja-JP~0.0.1.0"
)

foreach ($c in $Capabilities) {
  if ((Get-WindowsCapability -Online -Name $c).State -ne "Installed") {
    Add-WindowsCapability -Online -Name $c | Out-Null
  }
}

Set-WinSystemLocale -SystemLocale ja-JP
Set-WinUILanguageOverride -Language ja-JP
Set-WinUserLanguageList -LanguageList ja-JP, en-US -Force
Set-WinDefaultInputMethodOverride -InputTip "0411:00000411"
Set-Culture -CultureInfo ja-JP
Set-WinHomeLocation -GeoId 122
Set-TimeZone -Id "Tokyo Standard Time"

Copy-UserInternationalSettingsToSystem -WelcomeScreen $true -NewUser $true
shutdown /r /t 30
