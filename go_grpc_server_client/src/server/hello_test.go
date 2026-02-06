package main

import (
	"context"
	"testing"

	pb_hello "example.com/hello-grpc/src/proto/hello"
)

func TestGreeterServer_SayHello(t *testing.T) {
	s := &greeterServer{}
	req := &pb_hello.HelloRequest{Name: "TestUser"}
	resp, err := s.SayHello(context.Background(), req)
	if err != nil {
		t.Fatalf("SayHello failed: %v", err)
	}
	if resp.GetMessage() != "Hello, TestUser!" {
		t.Errorf("Expected message 'Hello, TestUser!', got '%s'", resp.GetMessage())
	}
}
