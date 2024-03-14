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
  templateUrl: './user-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserListComponent {

  @Input()
  users: User[] | null = [];

}
