package main

import (
	"log"
	"net"

	pb_hello "example.com/hello-grpc/src/proto/hello"
	pb_todo "example.com/hello-grpc/src/proto/todo"
	"google.golang.org/grpc"
)

func main() {
	lis, err := net.Listen("tcp", ":50051")
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	s := grpc.NewServer()

	// Register Services
	pb_hello.RegisterGreeterServer(s, &greeterServer{})
	pb_todo.RegisterTodoServiceServer(s, NewTodoServer())

	log.Printf("server listening at %v", lis.Addr())
	if err := s.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
}
