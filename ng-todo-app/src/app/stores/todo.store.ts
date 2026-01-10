import { effect, Injectable, signal } from '@angular/core';
import { Todo } from '../models/todo';

const STORAGE_KEY = 'ng-todo-app.todos';

@Injectable({ providedIn: 'root' })
export class TodoStore {
  private readonly _todos = signal<Todo[]>(this.loadFromStorage());
  readonly todos = this._todos.asReadonly();

  constructor() {
    effect(() => {
      const todos = this._todos();
      localStorage.setItem(STORAGE_KEY, JSON.stringify(todos));
    });
  }

  add(title: string) {
    this._todos.update(todos => [
      { id: crypto.randomUUID(), title, completed: false },
      ...todos
    ]);
  }
  toggle(id: string) {
    this._todos.update(todos =>
      todos.map(t =>
        t.id === id ? { ...t, completed: !t.completed } : t
      )
    );
  }
  remove(id: string) {
    this._todos.update(todos => todos.filter(t => t.id !== id));
  }

  private loadFromStorage(): Todo[] {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      return raw ? (JSON.parse(raw) as Todo[]) : [];
    } catch {
      return [];
    }
  }
}
