use axum::{
    extract::{Path, State},
    http::StatusCode,
    routing::get,
    Json, Router,
};
use serde::{Deserialize, Serialize};
use std::sync::{Arc, RwLock};
use std::net::SocketAddr;

// データ構造
#[derive(Debug, Serialize, Deserialize, Clone)]
struct Task {
    id: u64,
    title: String,
    completed: bool,
}

// 新規作成用の構造体
#[derive(Deserialize)]
struct CreateTask {
    title: String,
}

// 更新用の構造体
#[derive(Deserialize)]
struct UpdateTask {
    title: Option<String>,
    completed: Option<bool>,
}

// 共有状態（インメモリDB）
type Db = Arc<RwLock<Vec<Task>>>;

#[tokio::main]
async fn main() {
    let db = Db::default();

    // ルーティングの設定 (Axum 0.8+ では {id} を使用)
    let app = Router::new()
        .route("/tasks", get(list_tasks).post(create_task))
        .route("/tasks/{id}", get(get_task).put(update_task).delete(delete_task))
        .with_state(db);

    let addr = SocketAddr::from(([127, 0, 0, 1], 3000));
    println!("listening on {}", addr);
    let listener = tokio::net::TcpListener::bind(addr).await.unwrap();
    axum::serve(listener, app).await.unwrap();
}

// 一覧取得
async fn list_tasks(State(db): State<Db>) -> Json<Vec<Task>> {
    let tasks = db.read().unwrap();
    Json(tasks.clone())
}

// 新規作成
async fn create_task(
    State(db): State<Db>,
    Json(payload): Json<CreateTask>,
) -> (StatusCode, Json<Task>) {
    let mut tasks = db.write().unwrap();
    let task = Task {
        id: (tasks.len() as u64) + 1,
        title: payload.title,
        completed: false,
    };
    tasks.push(task.clone());
    (StatusCode::CREATED, Json(task))
}

// 1件取得
async fn get_task(
    Path(id): Path<u64>,
    State(db): State<Db>,
) -> Result<Json<Task>, StatusCode> {
    let tasks = db.read().unwrap();
    tasks
        .iter()
        .find(|t| t.id == id)
        .cloned()
        .map(Json)
        .ok_or(StatusCode::NOT_FOUND)
}

// 更新
async fn update_task(
    Path(id): Path<u64>,
    State(db): State<Db>,
    Json(payload): Json<UpdateTask>,
) -> Result<Json<Task>, StatusCode> {
    let mut tasks = db.write().unwrap();
    if let Some(task) = tasks.iter_mut().find(|t| t.id == id) {
        if let Some(title) = payload.title {
            task.title = title;
        }
        if let Some(completed) = payload.completed {
            task.completed = completed;
        }
        Ok(Json(task.clone()))
    } else {
        Err(StatusCode::NOT_FOUND)
    }
}

// 削除
async fn delete_task(
    Path(id): Path<u64>,
    State(db): State<Db>,
) -> StatusCode {
    let mut tasks = db.write().unwrap();
    let before_count = tasks.len();
    tasks.retain(|t| t.id != id);
    if tasks.len() < before_count {
        StatusCode::NO_CONTENT
    } else {
        StatusCode::NOT_FOUND
    }
}
