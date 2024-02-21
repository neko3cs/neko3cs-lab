# Send mail with .NET

ただの.NET MAUIの勉強とMailKitの使い方を兼ねたアプリです。

## 起動方法

vscodeで `Cmd+Shift+P` をして「.NET MAUI: スタートアップデバイスの選択」を選択し、任意のデバイスを選んだ上で `F5` でデバッグ実行できる。

なお、コマンドで実行する場合は以下の通り。

### iOS

以下のコマンドで実行する。

```zsh
dotnet build -t:Run -f net8.0-ios -p:_DeviceName=:v2:udid=MY_SPECIFIC_UDID
```

`MY_SPECIFIC_UDID` の部分は端末によって変わる。

以下のコマンドでUDIDを確認できる。

```zsh
xcrun simctl list -v devices available
```

### Android

Android StudioのDevice Managerから任意のエミュレーターを起動した状態で以下のコマンドを実行する。

```zsh
dotnet build -t:Run -f net8.0-android
```

## 参考資料

- [.NET CLI を使用して macOS で iOS アプリを構築する - .NET MAUI | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/maui/ios/cli)
- [Android Emulator でのデバッグ - .NET MAUI | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/maui/android/emulator/debug-on-emulator?view=net-maui-8.0)
- [jstedfast/MailKit: A cross-platform .NET library for IMAP, POP3, and SMTP.](https://github.com/jstedfast/MailKit)
- [.NET MAUI で MVVM パターンを書いてみよう - 放浪軍師のアプリ開発局](https://www.gunshi.info/entry/2022/07/01/090944)
