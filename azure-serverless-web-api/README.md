# Azure Serverless Web API

Azure Functions (分離モデル）とAzure SQL Databaseを使用した、サーバーレスなWeb APIのプロジェクトです。

## 特徴

- **構成**: Azure Functions (隔離プロセスモデル）, EF Core, SQL Database
- **セキュリティ**:
  - 閉域ネットワーク構成 (VNet Integration, Private Endpoints)。
  - Application Gateway + WAFによるロードバランシングと保護。
  - ミドルウェアによるAPIキー認証 (X-API-KEYヘッダー)。
- **実装**:
  - `GetDatabaseVersion`: データベースのバージョン情報を取得します。
  - `GetUsers`: `dbo.User` テーブルから全ユーザーを取得します (初回起動時に自動テーブル作成 & データ投入）。
- **インフラ**: Azure Bicepを使用したモジュール構成 (`Main.bicep`, `Network.bicep`, `Database.bicep`, `Application.bicep`, `AppGateway.bicep`)。
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
- Azure Functions Core Tools (v4以上推奨）

---

## 既知の課題と試行錯誤の記録：Key Vault の導入について

現在、セキュリティ向上のためにAzure Key Vaultへの移行を検討・試行しましたが、以下の理由により実装を保留しています。

### 発生している問題

Bicepによるインフラデプロイ時に、Key Vaultへのアクセス権限（RBAC）を設定する際、**`RoleDefinitionDoesNotExist`** エラーが繰り返し発生しました。

### 試行した内容

1.  **標準的なロール ID 指定**: `subscriptionResourceId` 関数を使用して組み込みロール「Key Vault Secrets Officer」を参照。
2.  **フルパスの直接指定**: `/subscriptions/{subId}/providers/Microsoft.Authorization/roleDefinitions/{guid}` の形式で指定。
3.  **テナントレベルのパス指定**: `/providers/Microsoft.Authorization/roleDefinitions/{guid}` の形式で指定。
4.  **リソース名のユニーク化**: 論理削除（Soft Delete）された同名Key Vaultとの衝突を避けるため、ランダムシード（nameSeed）を導入して名前を動的に生成。

### 現状の分析

- **ロール定義の解決失敗**: エラーメッセージにおいて、Bicepで指定したGUIDからハイフンが除去された状態で表示されるなど、Azure側でロールが正しく解決されていない。
- **再現性と保守性の懸念**: ハードコードや環境依存のパス指定による一時的な解決は、プロジェクトの健全性を損なう可能性があると判断。

### 今後の方針

現在は、透過的なKey Vault参照が可能な設計（ミドルウェアによる抽象化）を導入済みです。まずは安定したAPI動作を優先し、Key Vaultへの完全移行はAzure側のロール参照の挙動をさらに精査した上で行う予定です。
