# Storage bucket for function source code
resource "google_storage_bucket" "bucket" {
  name                        = "${var.project_id}-gcf-source"
  location                    = var.region
  uniform_bucket_level_access = true
  force_destroy               = true
}

# Create a simple dummy ZIP for initial deployment
data "archive_file" "dummy_zip" {
  type        = "zip"
  output_path = "${path.module}/dummy-source.zip"
  source {
    content  = "{ \"name\": \"app\", \"version\": \"1.0.0\", \"main\": \"index.js\" }"
    filename = "package.json"
  }
  source {
    content  = "exports.app = (req, res) => res.send('Initializing...');"
    filename = "index.js"
  }
}

# Dummy source code to initialize the function
resource "google_storage_bucket_object" "dummy_source" {
  name   = "dummy-source.zip"
  bucket = google_storage_bucket.bucket.name
  source = data.archive_file.dummy_zip.output_path
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
    vpc_connector_egress_settings = "PRIVATE_RANGES_ONLY"
    
    # Ingress limited to Internal + Load Balancer
    ingress_settings = "ALLOW_INTERNAL_AND_GCLB"
  }
}
