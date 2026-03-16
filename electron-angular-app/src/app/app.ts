import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [],
  template: `
    <h1>Hello, {{ title() }}</h1>
    
    <button (click)="ping()">Ping</button>
  `,
  styles: [],
})
export class App {
  protected readonly title = signal('electron-angular-app');

  ping() {
    window.electronAPI.ping();
  }
}
