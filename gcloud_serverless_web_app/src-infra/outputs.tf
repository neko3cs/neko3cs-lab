# Terraform 実行完了後に出力される情報の定義

output "lb_ip" {
  description = "ロードバランサーのIPアドレス。これを使ってアクセスします。"
  value       = google_compute_global_forwarding_rule.forwarding_rule.ip_address
}

output "db_instance_name" {
  description = "作成された Cloud SQL インスタンスの名前"
  value       = google_sql_database_instance.instance.name
}

output "function_url" {
  description = "Cloud Functions の内部URL（現在はIngress設定により直接アクセスは制限されています）"
  value       = google_cloudfunctions2_function.function.service_config[0].uri
}
