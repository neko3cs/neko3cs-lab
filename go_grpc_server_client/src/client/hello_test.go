package main

import (
	"testing"

	pb_hello "example.com/hello-grpc/src/proto/hello"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func TestCallSayHello(t *testing.T) {
	conn, err := grpc.NewClient("passthrough://bufnet", grpc.WithContextDialer(bufDialer), grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		t.Fatalf("Failed to dial bufnet: %v", err)
	}
	defer conn.Close()

	client := pb_hello.NewGreeterClient(conn)

	// Just verify it runs without panic
	callSayHello(client, "Tester")
}
