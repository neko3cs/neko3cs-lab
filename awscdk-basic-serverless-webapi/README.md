# AWS CDK Serverless WebAPI サンプルコード

AWS CDK v2を用いた、サーバーレスWeb APIインフラのIaC（Infrastructure as Code）サンプルプロジェクトです。

## はじめに

このプロジェクトは、AWS上にスケーラブルでセキュアなサーバーレスWeb API環境を構築するためのテンプレートです。VPC、Fargate、Aurora PostgreSQLを組み合わせた一般的な3層構造を採用しています。

## 構成

このインフラ構成は、マルチAZ冗長化とサブネットによる階層的なセキュリティ管理を行っています。

![Infrastructure Configuration](docs/infra-fig.png)

### 主な特徴

- **ネットワーク**: 2つのAZを利用した高可用性構成。
- **セキュリティ**: セキュリティグループによる最小権限の通信制御。
- **データベース**: 最新のAurora PostgreSQL (v17.7) を使用。
- **コンテナー**: Fargateを使用したサーバーレスな実行環境。

## 実行方法

### 準備

- Node.jsおよびpnpmのインストール
- AWS CLIのセットアップとログイン

### 手順

1. **依存パッケージのインストール**

   ```bash
   pnpm install
   ```

2. **TypeScript のビルド**

   ```bash
   pnpm build
   ```

3. **CDK ブートストラップ（未実施の場合のみ）**

   ```bash
   pnpm exec cdk bootstrap
   ```

4. **デプロイ**

   ```bash
   pnpm exec cdk deploy
   ```

## 削除方法

課金を防ぐため、不要になったリソースは以下のコマンドで削除してください。

```bash
pnpm exec cdk destroy
```

---

_参考文献_: [本番で使えるFargate環境構築をCDKでやってみる - 虎の穴ラボ技術ブログ](https://toranoana-lab.hatenablog.com/entry/2024/08/15/130000?_gl=1*1slex9h*_gcl_au*MTQ1NDgyNDA4MC4xNzIyOTI2Mzkw)
