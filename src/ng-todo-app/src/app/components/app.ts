import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  styles: [],
  template: `
    <h1>Hello, {{ title() }}</h1>
    <router-outlet></router-outlet>
  `,
  standalone: true,
})
export class App {
  protected readonly title = signal('ng-todo-app');
}
