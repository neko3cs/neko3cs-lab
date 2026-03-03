variable "project_id" {
  description = "Google Cloud Project ID"
  type        = string
  default     = "gcloud-serverless-web-app"
}

variable "region" {
  description = "Google Cloud Region"
  type        = string
  default     = "asia-northeast1"
}

variable "db_password" {
  description = "Database password (sent to Secret Manager)"
  type        = string
  sensitive   = true
  default     = "P@ssword123!"
}

variable "app_username" {
  description = "Username for Basic Auth"
  type        = string
  default     = "gclouduser"
}

variable "app_password" {
  description = "Password for Basic Auth"
  type        = string
  default     = "P@ssword"
}
