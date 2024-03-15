import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { User } from '../../types/user';

@Component({
  selector: 'user-list-item',
  standalone: true,
  imports: [
    RouterLink,
  ],
  template: `
    <a routerLink="users/{{user.id}}" routerLinkActive="active">
      #{{ user.id }} {{ user.name }}
    </a>
  `,
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserListItemComponent {

  @Input()
  user!: User;

}
