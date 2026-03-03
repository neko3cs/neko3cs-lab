# Storage bucket for function source code
resource "google_storage_bucket" "bucket" {
  name                        = "${var.project_id}-gcf-source"
  location                    = var.region
  uniform_bucket_level_access = true
  force_destroy               = true
}

# Dummy source code to initialize the function
resource "google_storage_bucket_object" "dummy_source" {
  name   = "dummy-source.zip"
  bucket = google_storage_bucket.bucket.name
  content = "dummy content" # In a real scenario, this would be a zip with package.json and index.js
}

# Cloud Functions Service Account
resource "google_service_account" "function_sa" {
  account_id   = "function-sa"
  display_name = "Cloud Function Service Account"
}

# Cloud Functions (2nd Gen)
resource "google_cloudfunctions2_function" "function" {
  name        = "app-function"
  location    = var.region
  description = "Cloud Function with DB connection"

  build_config {
    runtime     = "nodejs20"
    entry_point = "app"
    source {
      storage_source {
        bucket = google_storage_bucket.bucket.name
        object = google_storage_bucket_object.dummy_source.name
      }
    }
  }

  service_config {
    max_instance_count = 1
    available_memory   = "256Mi"
    timeout_seconds    = 60
    service_account_email = google_service_account.function_sa.email

    vpc_connector = google_vpc_access_connector.connector.id
    vpc_connector_egress_settings = "ALL_TRAFFIC"
    
    # Ingress limited to Internal + Load Balancer
    ingress_settings = "ALLOW_INTERNAL_AND_GCLB"
  }
}

# Permissions for Service Account
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
