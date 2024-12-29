import { Component } from '@angular/core';

@Component({
  selector: 'app-container',
  standalone: true,
  styles: [`
    .container {
      display: flex;
      justify-content: center;
      align-items: center;
      flex-direction: column;
      text-align: center;
      height: 100vh;
      gap: 20px;
    }
  `],
  template: `
    <div class="container">
      <ng-content></ng-content>
    </div>
  `,
})
export class ContainerComponent { }
