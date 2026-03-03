# Google Cloud サーバーレス Web API アプリケーション

このプロジェクトは、Google Cloudを使用してセキュリティを重視したサーバーレスなWeb API構成を構築するプロトタイプです。Terraformによるインフラ管理と、Node.js + TypeScriptによるアプリケーション実装を統合しています。

## 構成の概要

ロードバランサーとWAFで入口を制限し、アプリケーションとデータベースを閉域ネットワーク内に隔離した、エンタープライズ向けの標準的な構成を採用しています。

- **Frontend**: HTTP(S) Load Balancer + Cloud Armor (WAF) による不正アクセス制限。
- **Compute**: Cloud Functions (第2世代）を使用。
- **Database**: Cloud SQL (PostgreSQL 15) をプライベートIPのみで運用。
- **Network**: VPC, Serverless VPC Accessを使用し、FunctionsからSQLへの閉域接続を実現。
- **Security**: Secret Managerを使用し、DBパスワードやBasic認証情報を安全に管理。
- **Governance**: すべてのリソースに `stack_name` ラベルを付与し、一括管理が可能。

## テックスタック

- **Language**: Node.js 20, TypeScript
- **Infrastructure**: Terraform (>= 1.0)
- **Cloud Provider**: Google Cloud
- **Tools**: `gcloud` CLI, `curl`

## Terraform に関する Tips (学習のポイント)

今回の構築を通じて得られた主要なTipsです。

1. **`main.tf` は必須ではない**:
   Terraformはカレントディレクトリの `.tf` ファイルをすべてマージして読み込むため、`vpc.tf`, `database.tf` のように機能ごとに分割して管理しても問題ありません。これにより、コードの視認性が向上します。

2. **`default_labels` による一括タグ付け**:
   プロバイダー設定で `default_labels` を定義すると、対応するすべてのリソースに自動でラベルが付与されます。これにより、コンソールの「アセット インベントリ」などでTerraform管理リソースを特定しやすくなります。

3. **アプリケーションのデプロイ一本化**:
   `data "archive_file"` と `google_storage_bucket_object` を組み合わせ、ソースコードのMD5ハッシュをファイル名に含めることで、コードを修正した時だけ `terraform apply` が「更新」を検知し、Cloud Functionsのみをアップデートする構成にできました。

4. **API 有効化の伝播待ち**:
   新しいGoogle CloudプロジェクトでAPIを有効化した直後、権限の反映に数分かかることがあります。初期構築時にエラーが出た場合は、少し待ってから再試行するのが現実的です。

5. **VPC Egress (下り) 設定の注意点**:
   `vpc_connector_egress_settings` を `ALL_TRAFFIC` にすると、外部API（Secret Manager等）へのアクセスにもCloud NAT等が必要になります。今回は `PRIVATE_RANGES_ONLY` にすることで、DB接続のみVPCを通し、他は標準のインターネット経路を使うシンプルな構成にしました。

## 実行方法

### 1. インフラの構築とデプロイ
`src-infra` ディレクトリで以下のコマンドを実行します。

```bash
cd src-infra
terraform init
terraform apply
```

※ アプリケーションのコードを変更した場合も、同じディレクトリで `terraform apply` を実行するだけで、差分デプロイが行われます。

### 2. 動作確認
ロードバランサーのIPアドレス（`lb_ip`）に対して、Basic認証を指定してアクセスします。

```bash
# DBからPostgreSQLのバージョン情報を取得
curl -u gclouduser:P@ssword http://[LB_IP]/conn-db
```

### 3. リソースの確認
Google Cloudコンソールのアセット インベントリで、以下のラベルでフィルタリングすることで、構築されたリソースを一覧できます。
`labels.stack_name: gcloud-serverless-web-app`
