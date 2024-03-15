import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListItemComponent } from '../user-list-item/user-list-item.component';
import { User } from '../user';

@Component({
  selector: 'user-list',
  standalone: true,
  imports: [
    CommonModule,
    UserListItemComponent,
  ],
  template: `
    <ul>
      <li *ngFor="let user of users">
        <user-list-item [user]="user"></user-list-item>
      </li>
    </ul>
  `,
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserListComponent {

  @Input()
  users: User[] | null = [];

}
