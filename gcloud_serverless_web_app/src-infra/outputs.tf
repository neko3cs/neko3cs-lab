output "lb_ip" {
  description = "The IP address of the Load Balancer"
  value       = google_compute_global_forwarding_rule.forwarding_rule.ip_address
}

output "db_instance_name" {
  description = "The name of the Cloud SQL instance"
  value       = google_sql_database_instance.instance.name
}

output "function_url" {
  description = "The URL of the Cloud Function"
  value       = google_cloudfunctions2_function.function.service_config[0].uri
}
