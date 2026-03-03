# このプロジェクトで使用する変数の定義
# デフォルト値（default）を設定しておくことで、実行時の入力を省略できます

variable "project_id" {
  description = "操作対象のGoogle CloudプロジェクトID"
  type        = string
  default     = "gcloud-serverless-web-app"
}

variable "region" {
  description = "リソースを作成する地理的な場所（リージョン）"
  type        = string
  default     = "asia-northeast1" # 東京リージョン
}

variable "db_password" {
  description = "Cloud SQL（データベース）のログインパスワード"
  type        = string
  sensitive   = true # コンソール出力時に値を隠す設定
  default     = "P@ssword123!"
}

variable "app_username" {
  description = "Web APIのBasic認証で使用するユーザー名"
  type        = string
  default     = "gclouduser"
}

variable "app_password" {
  description = "Web APIのBasic認証で使用するパスワード"
  type        = string
  default     = "P@ssword"
}
