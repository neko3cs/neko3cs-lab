import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { User } from '../user';

@Component({
  selector: 'user-list-item',
  standalone: true,
  imports: [
    RouterLink,
  ],
  templateUrl: './user-list-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserListItemComponent {

  @Input()
  user!: User;

}
