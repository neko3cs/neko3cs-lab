# Storage bucket for function source code
resource "google_storage_bucket" "bucket" {
  name                        = "${var.project_id}-gcf-source"
  location                    = var.region
  uniform_bucket_level_access = true
  force_destroy               = true
}

# Automatically package the application source code
data "archive_file" "function_source" {
  type        = "zip"
  output_path = "${path.module}/function-source.zip"
  source_dir  = "${path.module}/../src-app"
  excludes    = ["node_modules", ".git", ".gcloudignore", "deploy.sh", "dist", "pnpm-lock.yaml"]
}

# Upload the ZIP to GCS with a name based on its content hash to trigger updates
resource "google_storage_bucket_object" "source_zip" {
  name   = "source-${data.archive_file.function_source.output_md5}.zip"
  bucket = google_storage_bucket.bucket.name
  source = data.archive_file.function_source.output_path
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
        object = google_storage_bucket_object.source_zip.name
      }
    }
  }

  service_config {
    max_instance_count = 1
    available_memory   = "256Mi"
    timeout_seconds    = 60
    service_account_email = google_service_account.function_sa.email

    vpc_connector = google_vpc_access_connector.connector.id
    vpc_connector_egress_settings = "PRIVATE_RANGES_ONLY"
    
    # Ingress limited to Internal + Load Balancer
    ingress_settings = "ALLOW_INTERNAL_AND_GCLB"
  }
}
