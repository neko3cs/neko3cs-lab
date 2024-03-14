import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Store } from '../store/store.service';
import { User } from '../../user';

@Injectable()
export class UserService {

  get users$() {
    return this.store.select(state => state.userList.items);
  }

  constructor(
    @Inject(HttpClient) private http: HttpClient,
    @Inject(Store) private store: Store
  ) { }

  async fetchUsers() {
    this.http.get<User[]>('https://jsonplaceholder.typicode.com/users')
      .subscribe({
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
        }
      });
  }
}
