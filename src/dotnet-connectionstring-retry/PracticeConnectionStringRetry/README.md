# README

接続文字列には以下のプロパティによって接続の意図しない切断に対してリトライを行う機能がある。
本プログラムはその機能を確認するためのテストプログラム。

- ConnectRetryCount
- ConnectRetryInterval

上記の機能については以下の記事が分かりやすかったのでリンクを貼っておく。

🔗[ConnectRetryCount/ConnectRetryIntervalを試してみる at SE の雑記](https://blog.engineer-memo.com/2015/12/11/connectretrycountconnectretryinterval%E3%82%92%E8%A9%A6%E3%81%97%E3%81%A6%E3%81%BF%E3%82%8B/)

## 動作準備

動作確認用にAdventureWorksを利用しているが、投げているのは `select GETDATE()` なので正直なんでもいい。
とりあえず、接続可能なSQL Serverを用意する。

🔗[AdventureWorks サンプル データベース - SQL Server | Microsoft Learn](https://learn.microsoft.com/ja-jp/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms)

## 確認方法

プログラムは1秒ごとにDBから時間を取得しているだけのもの。

SQL ServerのWindowsサービスを再起動したりすると瞬断障害の再現が出来るので、その際にリトライされていれば自動でプログラムは再開する。
リトライが出来ていなければ途中で落ちる。

## 確認結果

どうやらトランザクションを張るとこの機能が有効にならずリトライされなくなる。
原因は不明なので、別途調査してみる。（そのうち）
