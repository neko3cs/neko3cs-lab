# 機密情報（パスワードなど）の管理設定

# DBのパスワードを保存する「箱（Secret）」
resource "google_secret_manager_secret" "db_password" {
  secret_id = "db-password"
  replication {
    auto {} # 自動レプリケーション
  }
}

# 保存する具体的なパスワード値
resource "google_secret_manager_secret_version" "db_password_v1" {
  secret      = google_secret_manager_secret.db_password.id
  secret_data = var.db_password
}

# Basic認証用のユーザー名
resource "google_secret_manager_secret" "app_username" {
  secret_id = "app-username"
  replication {
    auto {}
  }
}

resource "google_secret_manager_secret_version" "app_username_v1" {
  secret      = google_secret_manager_secret.app_username.id
  secret_data = var.app_username
}

# Basic認証用のパスワード
resource "google_secret_manager_secret" "app_password" {
  secret_id = "app-password"
  replication {
    auto {}
  }
}

resource "google_secret_manager_secret_version" "app_password_v1" {
  secret      = google_secret_manager_secret.app_password.id
  secret_data = var.app_password
}

# DBのIPアドレスを保存するシークレット（database.tfで値が入ります）
resource "google_secret_manager_secret" "db_host" {
  secret_id = "db-host"
  replication {
    auto {}
  }
}
