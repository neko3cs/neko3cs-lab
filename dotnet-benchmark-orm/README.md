# .NET 10 ORMライブラリベンチマーク

以下のライブラリについてベンチマークを取得し、比較します。

1. SqlClient (Microsoft.Data.SqlClient)
1. Entity Framework Core
1. Dapper

## 検証方法

Microsoftより提供されている、AdventureWorksサンプルデータベース（AdventureWorks2025）を活用する。

[AdventureWorks サンプル データベース - SQL Server | Microsoft Learn](https://learn.microsoft.com/ja-jp/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms#download-backup-files)

`Person.Person` テーブル（約2万件）に対して全件SELECTを実行し、POCOオブジェクトへのマッピング速度を計測する。

### 実行環境

- .NET 10.0.2
- BenchmarkDotNet v0.15.8
- macOS Sequoia 15.7.4
- Intel Core i7-8559U CPU 2.70GHz

## 検証結果

5回の独立した試行（各試行でWarmup 3回、 Iteration 10回）を実施しました。計測値には環境要因によると思われる数%〜10%程度のブレが確認されています。

### 試行別平均実行時間 (ms)

| Method                                         | Run 1 | Run 2 | Run 3 | Run 4 | Run 5 | **Average** |
| :--------------------------------------------- | :---: | :---: | :---: | :---: | :---: | :---------: |
| Entity Framework Core (Raw SQL + AsNoTracking) | 831.7 | 797.1 | 793.9 | 876.0 | 785.3 |  **816.8**  |
| Dapper                                         | 815.5 | 796.5 | 787.3 | 866.9 | 824.3 |  **818.1**  |
| SqlCommand (SqlClient)                         | 822.9 | 774.4 | 883.0 | 756.8 | 903.8 |  **828.2**  |
| Entity Framework Core (Raw SQL)                | 838.4 | 807.2 | 815.0 | 892.5 | 821.3 |  **834.9**  |
| Entity Framework Core (Model)                  | 842.7 | 800.1 | 811.7 | 872.3 | 869.0 |  **839.2**  |
| Entity Framework Core (Model + AsNoTracking)   | 838.1 | 778.2 | 866.2 | 883.0 | 954.4 |  **864.0**  |

### 考察

- **全体傾向**: 5回の平均値で見ると、Entity Framework CoreのRaw SQL (`FromSqlRaw`) と `AsNoTracking` を組み合わせた手法がDapperと僅差でトップとなりました。
- **計測のブレ**: 同一環境・同一コードであっても、試行によって順位が大きく入れ替わる程度のブレ（平均値で約750ms〜950msの範囲）が発生しています。これはディスクI/O、ネットワーク、OSのバックグラウンド処理、およびSQL Serverの挙動（キャッシュ等）に起因するものと考えられます。
- **EF Core の進化**: .NET 10におけるEF CoreのRaw SQL実行は、非常に軽量なマッパーであるDapperと同等以上のパフォーマンスを発揮できるレベルに最適化されていることが伺えます。
- **SqlClient (SqlCommand)**: 手動マッピングは最速のポテンシャル（Run 4では756.8msで全試行中トップ）を持っていますが、試行ごとの変動がもっとも大きく、平均ではEF CoreやDapperに一歩譲る結果となりました。
- **AsNoTracking の効果**: 読み取り専用クエリにおいて `AsNoTracking` を使用することで、トラッキングのオーバーヘッドが削減され、パフォーマンスの向上が期待できることが再確認されました。
