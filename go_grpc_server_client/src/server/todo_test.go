package main

import (
	"context"
	"testing"

	pb_todo "example.com/hello-grpc/src/proto/todo"
)

func TestTodoServer_CRUD(t *testing.T) {
	s := NewTodoServer()
	ctx := context.Background()

	// 1. Create
	createReq := &pb_todo.CreateTodoRequest{
		Title:       "Test Todo",
		Description: "Test Description",
	}
	createResp, err := s.CreateTodo(ctx, createReq)
	if err != nil {
		t.Fatalf("CreateTodo failed: %v", err)
	}
	todoID := createResp.GetTodo().GetId()
	if todoID == "" {
		t.Fatal("Expected Todo ID to be non-empty")
	}

	// 2. Get
	getReq := &pb_todo.GetTodoRequest{Id: todoID}
	getResp, err := s.GetTodo(ctx, getReq)
	if err != nil {
		t.Fatalf("GetTodo failed: %v", err)
	}
	if getResp.GetTodo().GetTitle() != "Test Todo" {
		t.Errorf("Expected title 'Test Todo', got '%s'", getResp.GetTodo().GetTitle())
	}

	// 3. Update
	updateReq := &pb_todo.UpdateTodoRequest{
		Id:          todoID,
		Title:       "Updated Todo",
		Description: "Updated Description",
		Done:        true,
	}
	updateResp, err := s.UpdateTodo(ctx, updateReq)
	if err != nil {
		t.Fatalf("UpdateTodo failed: %v", err)
	}
	if updateResp.GetTodo().GetTitle() != "Updated Todo" {
		t.Errorf("Expected updated title 'Updated Todo', got '%s'", updateResp.GetTodo().GetTitle())
	}
	if !updateResp.GetTodo().GetDone() {
		t.Error("Expected Done to be true")
	}

	// 4. List
	listReq := &pb_todo.ListTodosRequest{}
	listResp, err := s.ListTodos(ctx, listReq)
	if err != nil {
		t.Fatalf("ListTodos failed: %v", err)
	}
	if len(listResp.GetTodos()) != 1 {
		t.Errorf("Expected 1 todo, got %d", len(listResp.GetTodos()))
	}

	// 5. Delete
	deleteReq := &pb_todo.DeleteTodoRequest{Id: todoID}
	deleteResp, err := s.DeleteTodo(ctx, deleteReq)
	if err != nil {
		t.Fatalf("DeleteTodo failed: %v", err)
	}
	if !deleteResp.GetSuccess() {
		t.Error("Expected Success to be true")
	}

	// Verify Delete
	_, err = s.GetTodo(ctx, getReq)
	if err == nil {
		t.Error("Expected error when getting deleted todo, got nil")
	}
}
