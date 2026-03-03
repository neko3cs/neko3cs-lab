# VPC Network
resource "google_compute_network" "vpc_network" {
  name                    = "main-vpc"
  auto_create_subnetworks = false
}

# Subnet for Cloud SQL
resource "google_compute_subnetwork" "db_subnet" {
  name          = "db-subnet"
  ip_cidr_range = "10.0.1.0/24"
  region        = var.region
  network       = google_compute_network.vpc_network.id
}

# Subnet for Serverless VPC Access
resource "google_compute_subnetwork" "serverless_subnet" {
  name          = "serverless-subnet"
  ip_cidr_range = "10.10.0.0/28"
  region        = var.region
  network       = google_compute_network.vpc_network.id
}

# Global Address for Private Services Access (Cloud SQL)
resource "google_compute_global_address" "private_ip_address" {
  name          = "private-ip-address"
  purpose       = "VPC_PEERING"
  address_type  = "INTERNAL"
  prefix_length = 16
  network       = google_compute_network.vpc_network.id
}

# Private Service Connection for Cloud SQL
resource "google_service_networking_connection" "private_vpc_connection" {
  network                 = google_compute_network.vpc_network.id
  service                 = "servicenetworking.googleapis.com"
  reserved_peering_ranges = [google_compute_global_address.private_ip_address.name]
}

# Serverless VPC Access Connector
resource "google_vpc_access_connector" "connector" {
  name          = "vpc-con"
  region        = var.region
  subnet {
    name = google_compute_subnetwork.serverless_subnet.name
  }
}
