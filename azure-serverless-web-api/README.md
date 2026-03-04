# Azure Serverless Web API

Azure Functions (分離モデル) とAzure SQL Databaseを使用した、サーバーレスなWeb APIのプロジェクトです。

## 特徴

- **構成**: Azure Functions (隔離プロセスモデル), EF Core, SQL Database
- **セキュリティ**:
  - 閉域ネットワーク構成 (VNet Integration, Private Endpoints)。
  - Application Gateway + WAFによるロードバランシングと保護。
  - ミドルウェアによるAPIキー認証 (X-API-KEYヘッダー)。
- **実装**:
  - `GetDatabaseVersion`: データベースのバージョン情報を取得します。
  - `GetUsers`: `dbo.User` テーブルから全ユーザーを取得します (初回起動時に自動テーブル作成 & データ投入)。
- **インフラ**: Azure Bicepを使用したモジュール構成 (`Main.bicep`)。
- **テスト**: xUnit, Shouldly, Moqを使用したユニットテスト。

## プロジェクト構成

- `src-app/`: アプリケーションコード (.NET 10.0)
- `src-infra/`: インフラ定義コード (Bicep)

## デプロイ手順

### 1. インフラの構築
`src-infra` ディレクトリでデプロイスクリプトを実行します。
```zsh
cd src-infra
./Deploy.ps1
```

### 2. アプリケーションのパブリッシュ
インフラ構築後、アプリケーションコードをAzureにアップロードします。
```zsh
cd src-app/AzureServerlessWebApi
func azure functionapp publish webapi-func
```

## API の利用方法

### IP アドレスの取得
Application Gateway (WAF) のパブリックIPを取得します。
```zsh
export AGW_IP=$(az network public-ip show --resource-group rg-azure-serverless-web-api --name webapi-pip --query ipAddress -o tsv)
```

### API の実行 (curl 例)
認証には `X-API-KEY` ヘッダーが必要です (デフォルト値: `DefaultApiKey123`)。

```zsh
# ユーザー一覧の取得
curl -i -H "X-API-KEY: DefaultApiKey123" http://$AGW_IP/api/GetUsers

# データベースのバージョン確認
curl -i -H "X-API-KEY: DefaultApiKey123" http://$AGW_IP/api/GetDatabaseVersion
```

## 動作要件

- .NET 10.0 SDK
- Azure CLI
- PowerShell (pwsh)
- Azure Functions Core Tools (v4以上推奨)
