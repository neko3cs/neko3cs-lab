import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './user';
import { UserListFilter } from './state';
import { UserListUsecase } from './usecases/user-list.usecase';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  users$ = this.userList.users$;
  userListFilter$ = this.userList.filter$;

  constructor(private userList: UserListUsecase) { }

  ngOnInit() {
    this.userList.fetchUsers();
  }

  setUserListFilter(value: UserListFilter) {
    this.userList.setNameFilter(value.nameFilter);
  }
}
