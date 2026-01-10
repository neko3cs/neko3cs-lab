import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Todo } from '../../models/todo';

@Component({
  selector: 'app-todo-item',
  standalone: true,
  imports: [
    MatCardModule,
    MatCheckboxModule,
    MatIconModule,
    MatButtonModule
  ],
  template: `
    <mat-card style="margin-bottom: .8rem; display: flex; flex-direction: row; align-items: center;">
      <mat-checkbox
        [checked]="todo.completed"
        (change)="toggle.emit(todo.id)">
      </mat-checkbox>
      <span
        [style.text-decoration]="todo.completed ? 'line-through' : 'none'"
        style="flex: 1; margin-left: .8rem;"
      >
        {{ todo.title }}
      </span>
      <button
        mat-icon-button
        color="warn"
        (click)="delete.emit(todo.id)"
      >
        <mat-icon>delete</mat-icon>
      </button>
    </mat-card>
  `
})
export class TodoItem {
  @Input() todo!: Todo;
  @Output() toggle = new EventEmitter<string>();
  @Output() delete = new EventEmitter<string>();
}
