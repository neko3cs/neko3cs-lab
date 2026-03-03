# Azure Serverless Web API

Azure Functions (分離モデル）とAzure SQL Databaseを使用した、サーバーレスなWeb APIのプロジェクトです。

## 特徴

- **構成**: Azure Functions (隔離プロセスモデル）, EF Core, SQL Database
- **セキュリティ**:
  - 認証情報 (接続文字列等）をAzure Key Vaultで管理。
  - 閉域ネットワーク構成 (VNet Integration, Private Endpoints)。
  - Application Gateway + WAFによるロードバランシングと保護。
- **実装**:
  - `GetDatabaseVersion`: データベースのバージョン情報を取得します。
  - `GetUsers`: `dbo.User` テーブルから全ユーザーを取得します。
- **インフラ**: Azure Bicepを使用し、`Deploy.ps1` で一括デプロイが可能です。
- **テスト**: xUnitとShouldlyを使用してFunctionsの動作を検証します。

## プロジェクト構成

- `src-app/`: アプリケーションコード (.NET 9.0)
- `src-infra/`: インフラ定義コード (Bicep)

## 動作要件

- .NET 9.0 SDK
- Azure CLI / PowerShell
- Azure Functions Core Tools
