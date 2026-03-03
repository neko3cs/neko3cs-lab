# IAM（権限管理）の設定
# アプリケーションが必要な情報（DBやパスワード）にアクセスするための許可証を発行します

# 1. Secret Manager へのアクセス権限（パスワード等の取得用）
resource "google_secret_manager_secret_iam_member" "db_password_access" {
  secret_id = google_secret_manager_secret.db_password.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.function_sa.email}"
}

resource "google_secret_manager_secret_iam_member" "db_host_access" {
  secret_id = google_secret_manager_secret.db_host.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.function_sa.email}"
}

resource "google_secret_manager_secret_iam_member" "app_username_access" {
  secret_id = google_secret_manager_secret.app_username.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.function_sa.email}"
}

resource "google_secret_manager_secret_iam_member" "app_password_access" {
  secret_id = google_secret_manager_secret.app_password.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.function_sa.email}"
}

# 2. Cloud SQL へのアクセス権限（データベース接続用）
resource "google_project_iam_member" "cloud_sql_client" {
  project = var.project_id
  role    = "roles/cloudsql.client"
  member  = "serviceAccount:${google_service_account.function_sa.email}"
}

# 3. ロードバランサーからの実行権限
# Ingress設定で制限をかけているため、実質的にLB経由のアクセスのみがアプリに到達します
resource "google_cloud_run_service_iam_member" "invoker" {
  location = google_cloudfunctions2_function.function.location
  project  = google_cloudfunctions2_function.function.project
  service  = google_cloudfunctions2_function.function.name
  role     = "roles/run.invoker"
  member   = "allUsers" # アプリ側でBasic認証を行うため、認可はアプリに任せます
}
