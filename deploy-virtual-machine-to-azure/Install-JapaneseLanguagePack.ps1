#Requires -PSEdition Desktop
$ErrorActionPreference = "Stop"

$OSCaption = (Get-CimInstance -ClassName Win32_OperatingSystem).Caption
$JapaneseKeyboardLayout = "0411:00000411"
$JapanGeoId = 122
$TimeZone = "Tokyo Standard Time"
$MaxRetryCount = 3
$RetryIntervalSeconds = 15

function Install-LanguageCapability {
  param([string]$Name)

  for ($attempt = 1; $attempt -le $MaxRetryCount; $attempt++) {
    $state = (Get-WindowsCapability -Online -Name $Name).State
    if ($state -eq "Installed") {
      return $true
    }
    try {
      Add-WindowsCapability -Online -Name $Name -ErrorAction Stop | Out-Null
    }
    catch {
      Write-Warning "[$Name] インストール試行 $attempt/$MaxRetryCount 回目に失敗しました: $($_.Exception.Message)"
    }
    Start-Sleep -Seconds $RetryIntervalSeconds
  }

  return ((Get-WindowsCapability -Online -Name $Name).State -eq "Installed")
}

if ($OSCaption -match "Microsoft Windows Server 2016") {
  lpksetup /i ja-JP /s
}
else {
  # After Windows Server 2016 OS
  $Capabilities = @(
    "Language.Pack~~~ja-JP~0.0.1.0"
    "Language.Basic~~~ja-JP~0.0.1.0"
    "Language.Basic.Typing~~~ja-JP~0.0.1.0"
    "Language.Handwriting~~~ja-JP~0.0.1.0"
    "Language.OCR~~~ja-JP~0.0.1.0"
    "Language.Speech~~~ja-JP~0.0.1.0"
    "Language.TextToSpeech~~~ja-JP~0.0.1.0"
    "Language.Fonts.Japanese~~~ja-JP~0.0.1.0"
  )

  $FailedCapabilities = @()
  foreach ($capability in $Capabilities) {
    if (-not (Install-LanguageCapability -Name $capability)) {
      $FailedCapabilities += $capability
    }
  }

  if ($FailedCapabilities.Count -gt 0) {
    Write-Warning "以下の言語パックは自動インストールに失敗しました。設定アプリの [Time & Language] > [Language] > [Preferred languages] > [Japanese] > [Options] から手動でダウンロードしてください:`n$($FailedCapabilities -join "`n")"
  }
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
