import { Component, signal } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TodoForm } from '../todo-form/todo-form';
import { TodoList } from '../todo-list/todo-list';
import { Todo } from '../../models/todo';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatToolbarModule,
    TodoForm,
    TodoList
  ],
  styles: [`
    .container {
      max-width: 600px;
      margin: 2rem auto;
      padding: 1rem;
    }
  `],
  template: `
    <mat-toolbar color="primary">
      <span>Angular Todo App</span>
    </mat-toolbar>
    <div class="container">
      <app-todo-form (addTodo)="addTodo($event)"></app-todo-form>
      <app-todo-list
        [todos]="todos()"
        (toggleTodo)="toggleTodo($event)"
        (deleteTodo)="deleteTodo($event)">
      </app-todo-list>
    </div>
  `
})
export class App {
  readonly todos = signal<Todo[]>([]);

  addTodo(title: string) {
    const newTodo: Todo = {
      id: crypto.randomUUID(),
      title: title.trim(),
      completed: false
    };
    this.todos.update(todos => [newTodo, ...todos]);
  }
  toggleTodo(id: string) {
    this.todos.update(todos =>
      todos.map(todo =>
        todo.id === id ? { ...todo, completed: !todo.completed } : todo
      )
    );
  }
  deleteTodo(id: string) {
    this.todos.update(todos => todos.filter(t => t.id !== id));
  }
}
