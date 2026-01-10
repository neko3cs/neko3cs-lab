import { Component, inject, signal } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TodoForm } from '../todo-form/todo-form';
import { TodoList } from '../todo-list/todo-list';
import { Todo } from '../../models/todo';
import { TodoStore } from '../../stores/todo.store';

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
        [todos]="store.todos()"
        (toggleTodo)="toggleTodo($event)"
        (deleteTodo)="deleteTodo($event)">
      </app-todo-list>
    </div>
  `
})
export class App {
  readonly store = inject(TodoStore);

  addTodo(title: string) {
    this.store.add(title);
  }

  toggleTodo(id: string) {
    this.store.toggle(id);
  }

  deleteTodo(id: string) {
    this.store.remove(id);
  }
}
