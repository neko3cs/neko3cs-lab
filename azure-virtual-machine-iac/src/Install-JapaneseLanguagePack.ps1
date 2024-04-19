#Requires -PSEdition Desktop

$LanguagePackUrl = "http://download.windowsupdate.com/c/msdownload/update/software/updt/2016/09/lp_9a666295ebc1052c4c5ffbfa18368dfddebcd69a.cab"
$LanguagePackFilePath = "$PWD\LanguagePackFile.cab"
$InputTipJapanese = "0411:00000411"

Set-WinUserLanguageList `
  -LanguageList ja-JP, en-US `
  -Force
Start-BitsTransfer `
  -Source $LanguagePackUrl `
  -Destination $LanguagePackFilePath `
  -Priority High
Add-WindowsPackage `
  -PackagePath $LanguagePackFilePath `
  -Online
Set-WinDefaultInputMethodOverride `
  -InputTip $InputTipJapanese
Set-WinLanguageBarOption `
  -UseLegacySwitchMode `
  -UseLegacyLanguageBar
Remove-Item `
  -Path $LanguagePackFilePath `
  -Force
Restart-Computer
