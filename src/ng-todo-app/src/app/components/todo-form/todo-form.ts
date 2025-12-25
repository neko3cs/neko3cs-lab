import { Component, computed, EventEmitter, Output, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  template: `
    <mat-form-field appearance="outline" style="width: 100%;">
      <input
        matInput
        [value]="title()"
        placeholder="Add new todo"
        (input)="onInput($event)"
      />

      @if (showError()) {
        <mat-error>
          1文字以上入力してください。
        </mat-error>
      }
    </mat-form-field>
    <button
      mat-raised-button
      color="primary"
      (click)="submit()"
      [disabled]="!canSubmit()"
    >
      Add
    </button>
  `
})
export class TodoForm {
  @Output() addTodo = new EventEmitter<string>();

  readonly title = signal('');
  readonly touched = signal(false);

  readonly isValid = computed(() => this.title().trim().length > 0);
  readonly showError = computed(() => this.touched() && !this.isValid());
  readonly canSubmit = computed(() => this.isValid());

  onInput(event: Event) {
    this.touched.set(true);
    this.title.set((event.target as HTMLInputElement).value);
  }

  submit() {
    if (!this.isValid()) return;
    this.addTodo.emit(this.title().trim());
    this.title.set('');
    this.touched.set(false);
  }
}
