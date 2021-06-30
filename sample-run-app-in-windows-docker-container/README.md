# SampleRunAppInWindowsDockerContainer

WindowsコンテナーでSQL ServerやWindows Service、WebAppが起動することを確認する用

## TODO

1. [x] とりあえず、動作するWebApiとWindows Serviceを作ってみる
1. [ ] ひとまず、SQLServerが入ったWindowsコンテナーDockerfileの実装
    - Windows Nano Server or Windows Server Core?
1. [ ] DB接続のある処理があるようにWebApiとWindows Serviceを改修
1. [ ] バイナリーをデプロイして実行するようにDockerfileを改修
1. [ ] 正常実行しているか、エラーがあればエラーが出力されるようにDockerfileを改修
