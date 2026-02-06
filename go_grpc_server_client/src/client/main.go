package main

import (
	"log"

	pb_hello "example.com/hello-grpc/src/proto/hello"
	pb_todo "example.com/hello-grpc/src/proto/todo"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func main() {
	conn, err := grpc.NewClient("localhost:50051", grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("did not connect: %v", err)
	}
	defer conn.Close()

	clientHello := pb_hello.NewGreeterClient(conn)
	runHello(clientHello)

	clientTodo := pb_todo.NewTodoServiceClient(conn)
	runTodo(clientTodo)
}
