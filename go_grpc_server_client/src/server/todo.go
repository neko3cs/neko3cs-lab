package main

import (
	"context"
	"errors"
	"fmt"
	"log"
	"sync"
	"time"

	pb_todo "example.com/hello-grpc/src/proto/todo"
)

// --- Todo Server ---
type todoServer struct {
	pb_todo.UnimplementedTodoServiceServer
	mu    sync.Mutex
	todos map[string]*pb_todo.Todo
}

func NewTodoServer() *todoServer {
	return &todoServer{
		todos: make(map[string]*pb_todo.Todo),
	}
}

func (s *todoServer) CreateTodo(ctx context.Context, req *pb_todo.CreateTodoRequest) (*pb_todo.CreateTodoResponse, error) {
	s.mu.Lock()
	defer s.mu.Unlock()

	id := fmt.Sprintf("%d", time.Now().UnixNano())
	todo := &pb_todo.Todo{
		Id:          id,
		Title:       req.GetTitle(),
		Description: req.GetDescription(),
		Done:        false,
	}
	s.todos[id] = todo
	log.Printf("Todo Created: %s", id)
	return &pb_todo.CreateTodoResponse{Todo: todo}, nil
}

func (s *todoServer) GetTodo(ctx context.Context, req *pb_todo.GetTodoRequest) (*pb_todo.GetTodoResponse, error) {
	s.mu.Lock()
	defer s.mu.Unlock()

	todo, exists := s.todos[req.GetId()]
	if !exists {
		return nil, errors.New("todo not found")
	}
	return &pb_todo.GetTodoResponse{Todo: todo}, nil
}

func (s *todoServer) ListTodos(ctx context.Context, req *pb_todo.ListTodosRequest) (*pb_todo.ListTodosResponse, error) {
	s.mu.Lock()
	defer s.mu.Unlock()

	var todos []*pb_todo.Todo
	for _, todo := range s.todos {
		todos = append(todos, todo)
	}
	return &pb_todo.ListTodosResponse{Todos: todos}, nil
}

func (s *todoServer) UpdateTodo(ctx context.Context, req *pb_todo.UpdateTodoRequest) (*pb_todo.UpdateTodoResponse, error) {
	s.mu.Lock()
	defer s.mu.Unlock()

	id := req.GetId()
	if _, exists := s.todos[id]; !exists {
		return nil, errors.New("todo not found")
	}

	updatedTodo := &pb_todo.Todo{
		Id:          id,
		Title:       req.GetTitle(),
		Description: req.GetDescription(),
		Done:        req.GetDone(),
	}
	s.todos[id] = updatedTodo
	log.Printf("Todo Updated: %s", id)
	return &pb_todo.UpdateTodoResponse{Todo: updatedTodo}, nil
}

func (s *todoServer) DeleteTodo(ctx context.Context, req *pb_todo.DeleteTodoRequest) (*pb_todo.DeleteTodoResponse, error) {
	s.mu.Lock()
	defer s.mu.Unlock()

	id := req.GetId()
	if _, exists := s.todos[id]; !exists {
		return nil, errors.New("todo not found")
	}
	delete(s.todos, id)
	log.Printf("Todo Deleted: %s", id)
	return &pb_todo.DeleteTodoResponse{Success: true}, nil
}
