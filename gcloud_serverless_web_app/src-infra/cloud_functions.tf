# Cloud Functions（アプリケーション）の定義

# ソースコードを保存するためのバケット（倉庫）
resource "google_storage_bucket" "bucket" {
  name                        = "${var.project_id}-gcf-source"
  location                    = var.region
  uniform_bucket_level_access = true
  force_destroy               = true # テスト用のため削除時に全消去
}

# 【重要】src-appフォルダの内容を自動でZIP圧縮します
# これにより、コードを変更するたびにTerraformが差分を検知します
data "archive_file" "function_source" {
  type        = "zip"
  output_path = "${path.module}/function-source.zip"
  source_dir  = "${path.module}/../src-app"
  # ZIPに含めないファイル/フォルダを指定
  excludes    = ["node_modules", ".git", ".gcloudignore", "deploy.sh", "dist", "pnpm-lock.yaml"]
}

# 作成したZIPファイルをバケットにアップロードします
# ファイル名にMD5ハッシュを含めることで、内容が変わると別ファイルとして認識（＝デプロイが走る）されます
resource "google_storage_bucket_object" "source_zip" {
  name   = "source-${data.archive_file.function_source.output_md5}.zip"
  bucket = google_storage_bucket.bucket.name
  source = data.archive_file.function_source.output_path
}

# アプリが動作するための専用のサービスアカウント（身分証）
resource "google_service_account" "function_sa" {
  account_id   = "function-sa"
  display_name = "Cloud Function Service Account"
}

# Cloud Functions 本体（第2世代）
resource "google_cloudfunctions2_function" "function" {
  name        = "app-function"
  location    = var.region
  description = "Cloud Function with DB connection"

  # ビルド（コンパイル）時の設定
  build_config {
    runtime     = "nodejs20"
    entry_point = "app" # プログラム内の呼び出し口
    source {
      storage_source {
        bucket = google_storage_bucket.bucket.name
        object = google_storage_bucket_object.source_zip.name
      }
    }
  }

  # 実行（動作）時の設定
  service_config {
    max_instance_count = 1     # 同時実行数を制限（コスト節約）
    available_memory   = "256Mi"
    timeout_seconds    = 60
    service_account_email = google_service_account.function_sa.email

    # 作成したVPCコネクタを使ってネットワークに接続
    vpc_connector = google_vpc_access_connector.connector.id
    # DB宛（プライベート）の通信のみVPCを経由する設定
    vpc_connector_egress_settings = "PRIVATE_RANGES_ONLY"
    
    # 外部からの入口制限（内部通信およびロードバランサー経由のみ許可）
    ingress_settings = "ALLOW_INTERNAL_AND_GCLB"
  }
}
