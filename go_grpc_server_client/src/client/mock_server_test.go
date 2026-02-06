package main

import (
	"context"
	"errors"
	"log"
	"net"

	pb_hello "example.com/hello-grpc/src/proto/hello"
	pb_todo "example.com/hello-grpc/src/proto/todo"
	"google.golang.org/grpc"
	"google.golang.org/grpc/test/bufconn"
)

const bufSize = 1024 * 1024

var lis *bufconn.Listener

func init() {
	lis = bufconn.Listen(bufSize)
	s := grpc.NewServer()
	pb_hello.RegisterGreeterServer(s, &mockGreeterServer{})
	pb_todo.RegisterTodoServiceServer(s, &mockTodoServer{todos: make(map[string]*pb_todo.Todo)})
	go func() {
		if err := s.Serve(lis); err != nil {
			log.Fatalf("Server exited with error: %v", err)
		}
	}()
}

func bufDialer(context.Context, string) (net.Conn, error) {
	return lis.Dial()
}

// --- Mock Servers ---

type mockGreeterServer struct {
	pb_hello.UnimplementedGreeterServer
}

func (s *mockGreeterServer) SayHello(ctx context.Context, in *pb_hello.HelloRequest) (*pb_hello.HelloResponse, error) {
	return &pb_hello.HelloResponse{Message: "Mock Hello " + in.GetName()}, nil
}

type mockTodoServer struct {
	pb_todo.UnimplementedTodoServiceServer
	todos map[string]*pb_todo.Todo
}

func (s *mockTodoServer) CreateTodo(ctx context.Context, req *pb_todo.CreateTodoRequest) (*pb_todo.CreateTodoResponse, error) {
	id := "mock-id"
	todo := &pb_todo.Todo{
		Id:          id,
		Title:       req.GetTitle(),
		Description: req.GetDescription(),
		Done:        false,
	}
	s.todos[id] = todo
	return &pb_todo.CreateTodoResponse{Todo: todo}, nil
}

func (s *mockTodoServer) GetTodo(ctx context.Context, req *pb_todo.GetTodoRequest) (*pb_todo.GetTodoResponse, error) {
	todo, ok := s.todos[req.GetId()]
	if !ok {
		return nil, errors.New("not found")
	}
	return &pb_todo.GetTodoResponse{Todo: todo}, nil
}

func (s *mockTodoServer) ListTodos(ctx context.Context, req *pb_todo.ListTodosRequest) (*pb_todo.ListTodosResponse, error) {
	var list []*pb_todo.Todo
	for _, v := range s.todos {
		list = append(list, v)
	}
	return &pb_todo.ListTodosResponse{Todos: list}, nil
}

func (s *mockTodoServer) UpdateTodo(ctx context.Context, req *pb_todo.UpdateTodoRequest) (*pb_todo.UpdateTodoResponse, error) {
	if _, ok := s.todos[req.GetId()]; !ok {
		return nil, errors.New("not found")
	}
	updated := &pb_todo.Todo{
		Id:          req.GetId(),
		Title:       req.GetTitle(),
		Description: req.GetDescription(),
		Done:        req.GetDone(),
	}
	s.todos[req.GetId()] = updated
	return &pb_todo.UpdateTodoResponse{Todo: updated}, nil
}

func (s *mockTodoServer) DeleteTodo(ctx context.Context, req *pb_todo.DeleteTodoRequest) (*pb_todo.DeleteTodoResponse, error) {
	if _, ok := s.todos[req.GetId()]; !ok {
		return nil, errors.New("not found")
	}
	delete(s.todos, req.GetId())
	return &pb_todo.DeleteTodoResponse{Success: true}, nil
}
