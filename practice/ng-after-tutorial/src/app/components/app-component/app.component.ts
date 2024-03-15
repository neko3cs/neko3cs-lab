import { Component, Inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from "@angular/common";
import { UserListFilter } from '../../types/state';
import { UserListUsecase } from '../../usecases/user-list.usecase';
import { Observable } from 'rxjs';
import { User } from '../../types/user';
import { UserListFilterComponent } from "../user-list-filter/user-list-filter.component";
import { UserListComponent } from "../user-list/user-list.component";
import { UserApiService } from '../../services/user-api/user-api.service';
import { HttpClient } from '@angular/common/http';
import { Store } from '../../services/store/store.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    UserListFilterComponent,
    UserListComponent,
  ],
  providers: [
    HttpClient,
    Store,
    UserListUsecase,
    UserApiService,
  ],
  template: `
    <user-list-filter [value]="userListFilter$ | async" (valueChange)="setUserListFilter($event)" />
    <user-list [users]="users$ | async" />

    <router-outlet />
  `,
  styles: ``
})
export class AppComponent {

  users$: Observable<User[]> = new Observable();
  userListFilter$: Observable<UserListFilter> = new Observable();

  constructor(
    @Inject(UserListUsecase) private userList: UserListUsecase
  ) {
    this.userList.users$.subscribe({
      next: (users: User[]) => {
        this.users$ = new Observable<User[]>(observable => observable.next(users));
      },
      error: error => {
        console.error(`ERROR: ${error}`);
      }
    });
    this.userList.filter$.subscribe({
      next: (userListFilter: UserListFilter) => {
        this.userListFilter$ = new Observable<UserListFilter>(observable => observable.next(userListFilter));
      },
      error: error => {
        console.error(`ERROR: ${error}`);
      }
    });
  }

  ngOnInit() {
    this.userList.fetchUsers();
  }

  setUserListFilter(value: UserListFilter) {
    this.userList.setNameFilter(value.nameFilter);
  }
}
