# Cloud SQL PostgreSQL Instance
resource "google_sql_database_instance" "instance" {
  name             = "db-instance"
  region           = var.region
  database_version = "POSTGRES_15"
  depends_on       = [google_service_networking_connection.private_vpc_connection]

  settings {
    tier = "db-f1-micro" # Minimum size for development
    ip_configuration {
      ipv4_enabled    = false
      private_network = google_compute_network.vpc_network.id
    }
  }
  deletion_protection = false # For this demo
}

# Cloud SQL Database
resource "google_sql_database" "database" {
  name     = "app-db"
  instance = google_sql_database_instance.instance.name
}

# Cloud SQL User
resource "google_sql_user" "users" {
  name     = "app-user"
  instance = google_sql_database_instance.instance.name
  password = google_secret_manager_secret_version.db_password_v1.secret_data
}

# Store DB Host in Secret Manager
resource "google_secret_manager_secret_version" "db_host_v1" {
  secret      = google_secret_manager_secret.db_host.id
  secret_data = google_sql_database_instance.instance.private_ip_address
}
