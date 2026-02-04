use axum::{
    body::Body,
    http::{Request, StatusCode},
};
use hello_rust::{Task, app};
use http_body_util::BodyExt;
use tower::ServiceExt;

#[tokio::test]
async fn test_crud_scenario() {
    let app = app();

    // 1. テストデータは3つPOSTする
    let titles = vec!["Task 1", "Task 2", "Task 3"];
    for title in &titles {
        let response = app
            .clone()
            .oneshot(
                Request::builder()
                    .method("POST")
                    .uri("/tasks")
                    .header("content-type", "application/json")
                    .body(Body::from(format!(r#"{{"title": "{}"}}"#, title)))
                    .unwrap(),
            )
            .await
            .unwrap();
        assert_eq!(response.status(), StatusCode::CREATED);
    }

    // 2. /tasks で複数取れることを確認する
    let response = app
        .clone()
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/tasks")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    assert_eq!(response.status(), StatusCode::OK);
    let body = response.into_body().collect().await.unwrap().to_bytes();
    let tasks: Vec<Task> = serde_json::from_slice(&body).unwrap();
    assert_eq!(tasks.len(), 3);
    assert_eq!(tasks[0].title, "Task 1");
    assert_eq!(tasks[1].title, "Task 2");
    assert_eq!(tasks[2].title, "Task 3");

    // 3. PUT で1個更新する (ID: 1 を更新)
    let response = app
        .clone()
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri("/tasks/1")
                .header("content-type", "application/json")
                .body(Body::from(
                    r#"{"title": "Task 1 Updated", "completed": true}"#,
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(response.status(), StatusCode::OK);

    // 4. /tasks/{id} で1個取得する→更新されていることを確認する
    let response = app
        .clone()
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/tasks/1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    assert_eq!(response.status(), StatusCode::OK);
    let body = response.into_body().collect().await.unwrap().to_bytes();
    let task: Task = serde_json::from_slice(&body).unwrap();
    assert_eq!(task.id, 1);
    assert_eq!(task.title, "Task 1 Updated");
    assert_eq!(task.completed, true);

    // 5. DELETE で1個削除する (ID: 1 を削除)
    let response = app
        .clone()
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri("/tasks/1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(response.status(), StatusCode::NO_CONTENT);

    // 6. /tasks で数が減っていることを確認する (3つ作成 - 1つ削除 = 2つ)
    let response = app
        .clone()
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/tasks")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    assert_eq!(response.status(), StatusCode::OK);
    let body = response.into_body().collect().await.unwrap().to_bytes();
    let tasks: Vec<Task> = serde_json::from_slice(&body).unwrap();
    assert_eq!(tasks.len(), 2);
    assert_eq!(tasks[0].id, 2);
    assert_eq!(tasks[1].id, 3);
}
