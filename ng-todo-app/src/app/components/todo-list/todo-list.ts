import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TodoItem } from '../todo-item/todo-item';
import { Todo } from '../../models/todo';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [TodoItem],
  template: `
    @if (todos.length === 0) {
      <p style="text-align: center; margin-top: 1rem;">
        No todos yet.
      </p>
    } @else {
      @for (todo of todos; track todo.id) {
        <app-todo-item
          [todo]="todo"
          (toggle)="toggleTodo.emit(todo.id)"
          (delete)="deleteTodo.emit(todo.id)">
        </app-todo-item>
      }
    }
  `
})
export class TodoList {
  @Input() todos: Todo[] = [];
  @Output() toggleTodo = new EventEmitter<string>();
  @Output() deleteTodo = new EventEmitter<string>();
}
