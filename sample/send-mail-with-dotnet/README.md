# Send mail with .NET

ただの.NET MAUIの勉強とMailKitの使い方を兼ねたアプリです。

## 起動方法

iOSでデバッグする場合は以下のコマンドを実行する。

```zsh
dotnet build -t:Run -f net8.0-ios -p:_DeviceName=:v2:udid=MY_SPECIFIC_UDID
```

`MY_SPECIFIC_UDID` の部分は端末によって変わる。

以下のコマンドでUDIDを確認できる。

```zsh
xcrun simctl list -v devices available
```

## TODO

- [ ] Androidがなぜか上手く起動しないので直す。SDKパスが上手く通ってないかも知れない。

## 参考資料

- [.NET CLI を使用して macOS で iOS アプリを構築する - .NET MAUI | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/maui/ios/cli)
