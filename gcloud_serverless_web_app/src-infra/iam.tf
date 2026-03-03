# Allow unauthenticated invocations (auth handled by the app)
resource "google_cloud_run_service_iam_member" "invoker" {
  location = google_cloudfunctions2_function.function.location
  project  = google_cloudfunctions2_function.function.project
  service  = google_cloudfunctions2_function.function.name
  role     = "roles/run.invoker"
  member   = "allUsers"
}
