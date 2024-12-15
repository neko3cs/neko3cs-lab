import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../../types/user';

const apiHost = 'https://jsonplaceholder.typicode.com';

@Injectable()
export class UserApiService {

  constructor(
    @Inject(HttpClient) private http: HttpClient
  ) { }

  async getAllUsers() {
    return this.http.get<User[]>(`${apiHost}/users`);
  }

  async getUserById(id: string) {
    return this.http.get<User>(`${apiHost}/users/${id}`);
  }
}
