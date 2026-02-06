package main

import (
	"context"
	"log"

	pb_hello "example.com/hello-grpc/src/proto/hello"
)

// --- Greeter Server ---
type greeterServer struct {
	pb_hello.UnimplementedGreeterServer
}

func (s *greeterServer) SayHello(ctx context.Context, in *pb_hello.HelloRequest) (*pb_hello.HelloResponse, error) {
	log.Printf("Greeter Received: %v", in.GetName())
	return &pb_hello.HelloResponse{Message: "Hello, " + in.GetName() + "!"}, nil
}
