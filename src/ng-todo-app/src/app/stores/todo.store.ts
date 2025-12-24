import { signal } from '@angular/core';
import { Todo } from '../models/todo';

export class TodoStore {
  readonly todos = signal<Todo[]>([]);

  add(title: string) {
    this.todos.update(todos => [
      {
        id: crypto.randomUUID(),
        title,
        completed: false
      },
      ...todos
    ]);
  }
  toggle(id: string) {
    this.todos.update(todos =>
      todos.map(t =>
        t.id === id ? { ...t, completed: !t.completed } : t
      )
    );
  }
  remove(id: string) {
    this.todos.update(todos => todos.filter(t => t.id !== id));
  }
}
