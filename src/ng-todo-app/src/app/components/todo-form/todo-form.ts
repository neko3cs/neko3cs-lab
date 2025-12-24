import { Component, EventEmitter, Output } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FormsModule
  ],
  template: `
    <mat-form-field appearance="outline" style="width: 100%;">
      <input
        matInput
        [(ngModel)]="title"
        placeholder="Add new todo"
        (keyup.enter)="onAdd()"
      />
    </mat-form-field>
    <button
      mat-raised-button
      color="primary"
      (click)="onAdd()"
      [disabled]="!title.trim()"
    >
      Add
    </button>
  `
})
export class TodoForm {
  title = '';

  @Output() addTodo = new EventEmitter<string>();

  onAdd() {
    if (!this.title.trim()) return;
    this.addTodo.emit(this.title);
    this.title = '';
  }
}
