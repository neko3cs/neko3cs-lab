# WSL ContainerでRedmineを動かす

WSL ContainerでRedmineを動かしてみたときのサンプルコードです。Docker Hubの公式イメージにEVMプラグインを追加し、SQLiteのDBと添付ファイルをWindows側に保存します。

## 学びどころ

- `FROM` でRedmineの公式イメージを継承し、プラグインを追加してから `bundle install` で必要なgemを組み込みます。Redmineとプラグインは、動作確認したバージョンを指定しています。
- 公式イメージの `ENTRYPOINT` がDBのマイグレーションを行います。`REDMINE_PLUGINS_MIGRATE=1` でプラグインのマイグレーションも実行し、`CMD` で画像やスタイルシートをコンパイルしてからPumaを起動します。
- `--mount type=bind` を使うと、コンテナからWindows側のフォルダーに直接書き込めます。SQLiteのDBと添付ファイルを別々にマウントしているので、同じフォルダーを指定すればコンテナを作り直してもデータを引き継げます。

## 実行方法

WSL 2.9.3以降が必要です。`wsl --version` でバージョンを確認し、必要に応じて `wsl --update` を実行します。

### イメージのビルドと起動

PowerShellでこのディレクトリを開いて `Build-Redmine.ps1` を実行します。イメージをビルドして、`C:\redmine` を保存先にコンテナを起動します。保存先やポートを変える場合は `-DataPath` や `-Port` で指定できます。

```powershell
./Build-Redmine.ps1
wslc logs -f redmine-evm
```

### 初期設定

ログにPumaの起動が表示されたら、`Ctrl+C` でログ表示を終了します。新規のDBには、以下のコマンドで日本語の初期データを投入します。

```powershell
wslc exec -e REDMINE_LANG=ja redmine-evm bundle exec rake redmine:load_default_data
```

1. [http://127.0.0.1:3000/](http://127.0.0.1:3000/) を開き、`admin` / `admin` でログインしてパスワードを変更します。
2. 「管理 → プラグイン」で `Redmine Issue Evm plugin` を確認します。
3. プロジェクトの設定でEVMモジュールを有効にし、EVMタブで基準工数や地域などの初期設定を保存します。開始日・期日・予定工数の入ったチケットを作ると、EVMのグラフを試せます。

### データが残ることを確認する

チケットを作ってファイルを添付したら、コンテナを停止して削除します。

```powershell
wslc stop redmine-evm
wslc rm redmine-evm
```

同じ保存先を指定して `Build-Redmine.ps1` を実行し、チケットと添付ファイルが残っていることを確認します。DBの実体は `C:\redmine\sqlite\redmine.db`、添付ファイルの保存先は `C:\redmine\files` です。既存の `redmine.env` はスクリプトが再利用します。Railsのシークレットが入っているので、このファイルはGitの管理対象から外します。

## 参考サイト

- [無料でMS Projectっぽく使いたくて、WSL(Fedora)にRedmineを立てた話](https://zenn.dev/neko3cs/articles/redmine-wsl-install)
- [WSL Container - Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/wsl/wsl-container)
- [Redmine公式イメージ - Docker Hub](https://hub.docker.com/_/redmine)
- [redmine_issue_evm](https://github.com/momibun926/redmine_issue_evm/tree/v7.0.0)
