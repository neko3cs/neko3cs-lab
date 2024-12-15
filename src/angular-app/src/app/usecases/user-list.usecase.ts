import { Inject, Injectable } from '@angular/core';
import { Store } from '../services/store/store.service';
import { UserApiService } from '../services/user-api/user-api.service';
import { User } from '../types/user';
import { UserListFilter } from '../types/state';

@Injectable()
export class UserListUsecase {

  get users$() {
    return this.store
      .select<User[]>(state => state.userList.items.filter(user => user.name.includes(state.userListFilter.nameFilter)));
  }

  get filter$() {
    return this.store.select<UserListFilter>(state => state.userListFilter);
  }

  constructor(
    @Inject(UserApiService) private userApi: UserApiService,
    @Inject(Store) private store: Store
  ) { }

  async fetchUsers() {
    (await this.userApi.getAllUsers()).subscribe({
      next: (users: User[]) => {
        this.store.update(state => ({
          ...state,
          userList: {
            ...state.userList,
            items: users,
          }
        }));
      },
      error: error => {
        console.error(`ERROR: ${error}`);
      },
    });
  }

  setNameFilter(nameFilter: string) {
    this.store.update(state => ({
      ...state,
      userListFilter: {
        nameFilter
      }
    }));
  }
}
