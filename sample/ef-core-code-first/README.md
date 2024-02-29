# Entity Framework Code コードファーストサンプルコード

EF Coreのコードファースト実装のサンプルコードです。

## EF Core導入方法

以下のコマンドでパッケージを導入する。

```sh
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

また、CLIツールも導入すると便利。

```sh
dotnet tool install --global dotnet-ef
```

## サンプルアプリ実行方法

データベースを用意する。

dockerのSQL Server on Linuxを使った場合は以下の通り。

```sh
docker pull mcr.microsoft.com/mssql/server:latest &&
docker run `
  -e 'ACCEPT_EULA=Y' `
  -e 'SA_PASSWORD=P@ssword!' `
  -p 1433:1433 `
  --name sql1 `
  -v ${PWD}:/tmp `
  -d mcr.microsoft.com/mssql/server:latest
```

任意のクライアントから以下のSQL文を実行する。

```sql
drop database if exists [EfCoreCodeFirst]
create database [EfCoreCodeFirst]
```

プログラムを実行する。

要所要所で `Console.ReadLine()` しているので、SQLを発行してDBの状態を見るとよろし。

## 参考資料

- [概要 - EF Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/ef/core/get-started/overview/first-app?tabs=netcore-cli)
- [Entiy Framework Core のインストール - EF Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/ef/core/get-started/overview/install)
- [Create API と Drop API - EF Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/ef/core/managing-schemas/ensure-created)

