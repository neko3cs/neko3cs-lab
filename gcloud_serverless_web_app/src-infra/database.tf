# データベース（Cloud SQL）の設定

# Cloud SQL インスタンス本体
resource "google_sql_database_instance" "instance" {
  name             = "db-instance"
  region           = var.region
  database_version = "POSTGRES_15"
  
  # VPCピアリングが完了してから作成を開始する
  depends_on       = [google_service_networking_connection.private_vpc_connection]

  settings {
    tier = "db-f1-micro" # 開発用の最小構成サイズ（最も安価）
    
    # リソースを判別するためのラベル
    user_labels = {
      managed_by = "terraform"
      stack_name = "gcloud-serverless-web-app"
    }

    # IPアドレスの設定
    ip_configuration {
      ipv4_enabled    = false # 公開IPを無効化（プライベートのみ）
      private_network = google_compute_network.vpc_network.id # 自分のVPCに接続
    }
  }
  
  # デモ用のため、Terraform削除時にデータも削除できるように設定
  deletion_protection = false 
}

# インスタンス内に作成するデータベース
resource "google_sql_database" "database" {
  name     = "app-db"
  instance = google_sql_database_instance.instance.name
}

# データベースにアクセスするためのユーザー
resource "google_sql_user" "users" {
  name     = "app-user"
  instance = google_sql_database_instance.instance.name
  # パスワードはSecret Managerに保存されている値を参照します
  password = google_secret_manager_secret_version.db_password_v1.secret_data
}

# 作成されたDBのプライベートIPアドレスを、後でアプリが使えるようにSecret Managerに保存します
resource "google_secret_manager_secret_version" "db_host_v1" {
  secret      = google_secret_manager_secret.db_host.id
  secret_data = google_sql_database_instance.instance.private_ip_address
}
