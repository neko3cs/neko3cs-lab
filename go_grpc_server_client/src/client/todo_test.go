package main

import (
	"testing"

	pb_todo "example.com/hello-grpc/src/proto/todo"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func TestTodoScenario(t *testing.T) {
	conn, err := grpc.NewClient("passthrough://bufnet", grpc.WithContextDialer(bufDialer), grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		t.Fatalf("Failed to dial bufnet: %v", err)
	}
	defer conn.Close()

	client := pb_todo.NewTodoServiceClient(conn)

	// Test Create
	id := createTodo(client, "Test Title", "Test Desc")
	if id != "mock-id" {
		t.Errorf("Expected mock-id, got %s", id)
	}

	// Test Get
	getTodo(client, id)

	// Test Update
	updateTodo(client, id, "New Title", "New Desc", true)

	// Test List
	listTodos(client)

	// Test Delete
	deleteTodo(client, id)
}
