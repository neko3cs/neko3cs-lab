import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { ContainerComponent } from './container/container.component';
import { Greet } from '../../wailsjs/go/main/App';

@Component({
  standalone: true,
  selector: 'app-root',
  imports: [
    CommonModule,
    MatButtonModule,
    ContainerComponent,
  ],
  template: `
    <main>
      <app-container>
        <h1>{{ title }}</h1>
        <h2>{{ count }}</h2>
        <p>{{ greet }}</p>
        <div [ngStyle]="{ display: 'flex', gap: '20px' }">
          <button mat-raised-button color="primary" (click)="increment()">Increment</button>
          <button mat-raised-button color="primary" (click)="decrement()">Decrement</button>
        </div>
        <button mat-raised-button color="accent" (click)="reset()">Reset</button>
      </app-container>
    </main>
  `,
})
export class AppComponent {
  title = 'Wails Angular App';
  greet = 'Hello from Angular!';
  count = 0;

  async ngOnInit() {
    this.greet = await Greet('neko3cs');
  }

  increment() {
    this.count++;
  }

  decrement() {
    this.count--;
  }

  reset() {
    this.count = 0;
  }
}
