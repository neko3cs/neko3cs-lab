import { Component, signal, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [MatButtonModule, MatSnackBarModule],
  template: `
    <main class="flex min-h-screen flex-col items-center justify-center gap-10 text-center">

      <h1 class="text-5xl font-semibold tracking-tight">
        {{ title() }}
      </h1>

      <button
        mat-raised-button
        color="primary"
        class="px-6 py-3 text-lg"
        (click)="ping()"
      >
        Ping Electron
      </button>

    </main>
  `,
})
export class App {
  protected readonly title = signal('Electron Angular App');
  private snackBar = inject(MatSnackBar);

  async ping() {
    const response = await window.electronAPI.ping();

    this.snackBar.open(
      `Electron: ${response}`,
      'OK',
      {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom'
      }
    );
  }
}