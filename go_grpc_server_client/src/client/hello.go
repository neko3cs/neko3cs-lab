package main

import (
	"context"
	"log"
	"time"

	pb_hello "example.com/hello-grpc/src/proto/hello"
)

func runHello(c pb_hello.GreeterClient) {
	log.Println("--- Hello Scenario ---")
	callSayHello(c, "World")
	log.Println()
}

func callSayHello(c pb_hello.GreeterClient, name string) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()
	r, err := c.SayHello(ctx, &pb_hello.HelloRequest{Name: name})
	if err != nil {
		log.Printf("could not greet: %v", err)
		return
	}
	log.Printf("Greeting: %s", r.GetMessage())
}
