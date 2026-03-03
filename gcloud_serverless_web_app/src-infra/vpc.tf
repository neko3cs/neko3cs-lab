# ネットワーク関連のリソース定義

# プロジェクト専用の仮想ネットワーク（VPC）を作成します
resource "google_compute_network" "vpc_network" {
  name                    = "main-vpc"
  auto_create_subnetworks = false # サブネットは手動で指定します
}

# データベース（Cloud SQL）が属するサブネットワーク
resource "google_compute_subnetwork" "db_subnet" {
  name          = "db-subnet"
  ip_cidr_range = "10.0.1.0/24" # ネットワーク内のアドレス範囲
  region        = var.region
  network       = google_compute_network.vpc_network.id
}

# Serverless VPC Access（Cloud Functionsからプライベート接続するため）のサブネットワーク
resource "google_compute_subnetwork" "serverless_subnet" {
  name          = "serverless-subnet"
  ip_cidr_range = "10.10.0.0/28" # 必要最小限のサイズ (/28) 
  region        = var.region
  network       = google_compute_network.vpc_network.id
}

# Cloud SQLをプライベートIPのみで運用するための内部IPアドレス予約
resource "google_compute_global_address" "private_ip_address" {
  name          = "private-ip-address"
  purpose       = "VPC_PEERING" # VPCピアリング用
  address_type  = "INTERNAL"
  prefix_length = 16
  network       = google_compute_network.vpc_network.id
}

# Google側の管理ネットワークと自分のVPCを接続（VPCピアリング）
resource "google_service_networking_connection" "private_vpc_connection" {
  network                 = google_compute_network.vpc_network.id
  service                 = "servicenetworking.googleapis.com"
  reserved_peering_ranges = [google_compute_global_address.private_ip_address.name]
}

# Cloud Functionsを自分のVPCに接続するためのコネクタ
# これによりFunctionsがインターネットではなく、社内LANのような感覚でDBにアクセスできます
resource "google_vpc_access_connector" "connector" {
  name          = "vpc-con"
  region        = var.region
  subnet {
    name = google_compute_subnetwork.serverless_subnet.name
  }
}
