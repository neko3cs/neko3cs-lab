# Terraform自体の設定
terraform {
  # 実行に必要なTerraformの最小バージョン
  required_version = ">= 1.0"
  
  # 使用する外部プログラム（プロバイダー）の定義
  required_providers {
    # Google Cloudを操作するためのプロバイダー
    google = {
      source  = "hashicorp/google"
      version = "~> 5.0"
    }
    # ファイルをZIP圧縮するためのプロバイダー
    archive = {
      source  = "hashicorp/archive"
      version = "~> 2.4"
    }
  }
}

# Google Cloudプロバイダーの具体的な設定
provider "google" {
  project = var.project_id
  region  = var.region
  
  # ここで設定したラベルは、作成されるすべてのリソースに自動で付与されます
  default_labels = {
    managed_by = "terraform"
    stack_name = "gcloud-serverless-web-app"
  }
}
