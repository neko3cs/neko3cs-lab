package main

import (
	"context"
	"log"
	"time"

	pb_todo "example.com/hello-grpc/src/proto/todo"
)

func runTodo(c pb_todo.TodoServiceClient) {
	log.Println("--- Todo Scenario ---")

	// 1. Create
	createdTodoId := createTodo(c, "Buy Milk", "Need to buy 2L milk")

	// 2. List
	listTodos(c)

	// 3. Get
	getTodo(c, createdTodoId)

	// 4. Update
	updateTodo(c, createdTodoId, "Buy Milk & Eggs", "Need 2L milk and 10 eggs", true)

	// 5. List again
	listTodos(c)

	// 6. Delete
	deleteTodo(c, createdTodoId)

	// 7. List final
	listTodos(c)
	log.Println()
}

func createTodo(c pb_todo.TodoServiceClient, title, description string) string {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()
	createRes, err := c.CreateTodo(ctx, &pb_todo.CreateTodoRequest{
		Title:       title,
		Description: description,
	})
	if err != nil {
		log.Fatalf("could not create todo: %v", err)
	}
	createdTodoId := createRes.GetTodo().GetId()
	log.Printf("Created Todo: ID=%s, Title=%s", createdTodoId, createRes.GetTodo().GetTitle())
	return createdTodoId
}

func listTodos(c pb_todo.TodoServiceClient) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()
	listRes, err := c.ListTodos(ctx, &pb_todo.ListTodosRequest{})
	if err != nil {
		log.Printf("could not list todos: %v", err)
		return
	}
	log.Printf("List Todos (%d items):", len(listRes.GetTodos()))
	for _, t := range listRes.GetTodos() {
		log.Printf(" - [%s] %s (%v)", t.GetId(), t.GetTitle(), t.GetDone())
	}
}

func getTodo(c pb_todo.TodoServiceClient, id string) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()
	getRes, err := c.GetTodo(ctx, &pb_todo.GetTodoRequest{Id: id})
	if err != nil {
		log.Printf("could not get todo: %v", err)
		return
	}
	t := getRes.GetTodo()
	log.Printf("Got Todo: ID=%s, Title=%s, Done=%v", t.GetId(), t.GetTitle(), t.GetDone())
}

func updateTodo(c pb_todo.TodoServiceClient, id, title, description string, done bool) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()
	updateRes, err := c.UpdateTodo(ctx, &pb_todo.UpdateTodoRequest{
		Id:          id,
		Title:       title,
		Description: description,
		Done:        done,
	})
	if err != nil {
		log.Printf("could not update todo: %v", err)
		return
	}
	t := updateRes.GetTodo()
	log.Printf("Updated Todo: ID=%s, Title=%s, Done=%v", t.GetId(), t.GetTitle(), t.GetDone())
}

func deleteTodo(c pb_todo.TodoServiceClient, id string) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()
	deleteRes, err := c.DeleteTodo(ctx, &pb_todo.DeleteTodoRequest{Id: id})
	if err != nil {
		log.Printf("could not delete todo: %v", err)
		return
	}
	if deleteRes.GetSuccess() {
		log.Printf("Deleted Todo: ID=%s", id)
	} else {
		log.Printf("Failed to delete Todo: ID=%s", id)
	}
}
