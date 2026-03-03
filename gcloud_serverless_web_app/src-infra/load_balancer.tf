# ロードバランサーとWAF（セキュリティ）の設定

# Cloud Armor（WAF）のセキュリティポリシー
# ここで不正なアクセスを遮断します
resource "google_compute_security_policy" "policy" {
  name        = "waf-policy"
  description = "基本的なWAFポリシー設定"

  # 特定のIPを拒否するルールの例（現在はダミー）
  rule {
    action   = "deny(403)"
    priority = "1000"
    match {
      versioned_expr = "SRC_IPS_V1"
      config {
        src_ip_ranges = ["1.1.1.1/32"] # ここに拒否したいIPを記述
      }
    }
    description = "特定のIPからのアクセスを拒否"
  }

  # デフォルトではすべてのアクセスを許可（最後に評価されます）
  rule {
    action   = "allow"
    priority = "2147483647"
    match {
      versioned_expr = "SRC_IPS_V1"
      config {
        src_ip_ranges = ["*"]
      }
    }
    description = "デフォルトの許可ルール"
  }
}

# サーバーレス ネットワーク エンドポイント グループ（NEG）
# ロードバランサーとCloud Functionsを繋ぐための「窓口」
resource "google_compute_region_network_endpoint_group" "serverless_neg" {
  name                  = "serverless-neg"
  network_endpoint_type = "SERVERLESS"
  region                = var.region
  cloud_run {
    service = google_cloudfunctions2_function.function.name
  }
}

# バックエンドサービスの設定
resource "google_compute_backend_service" "backend" {
  name                  = "backend-service"
  protocol              = "HTTP"
  load_balancing_scheme = "EXTERNAL_MANAGED"
  security_policy       = google_compute_security_policy.policy.id

  backend {
    group = google_compute_region_network_endpoint_group.serverless_neg.id
  }
}

# URLマップ（どのURLをどのバックエンドに飛ばすかの地図）
resource "google_compute_url_map" "url_map" {
  name            = "url-map"
  default_service = google_compute_backend_service.backend.id
}

# HTTPプロキシ
resource "google_compute_target_http_proxy" "http_proxy" {
  name    = "http-proxy"
  url_map = google_compute_url_map.url_map.id
}

# グローバル転送ルール（インターネットからの入口となるIPアドレスとポートの設定）
resource "google_compute_global_forwarding_rule" "forwarding_rule" {
  name                  = "forwarding-rule"
  target                = google_compute_target_http_proxy.http_proxy.id
  port_range            = "80" # 今回はHTTP(80)で受け付けます
  load_balancing_scheme = "EXTERNAL_MANAGED"
}
