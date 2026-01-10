# .NET9 ORMライブラリベンチマーク

以下のライブラリについてベンチマークを取得し、比較します。

1. System.Data.SQL
1. Entity Framework Core
1. Dapper

## 検証方法

Microsoftより提供されている、AdventureWorksDW2019.bakを活用する。

[AdventureWorks サンプル データベース - SQL Server | Microsoft Learn](https://learn.microsoft.com/ja-jp/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms#download-backup-files)

DimCustomerテーブルに対してCRUD処理を実行してみる。

DimCustomerテーブルを使う理由は特にない。（18,000件データがあってちょうどよかったから）

## 検証結果

※検証中...
