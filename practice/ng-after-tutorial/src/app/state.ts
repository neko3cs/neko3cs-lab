import { User, initialUser } from './user';

export interface UserListFilter {
  nameFilter: string;
}

export interface State {
  userList: {
    items: User[];
  };
  userListFilter: UserListFilter;
  userDetail: {
    user: User;
  };
}

export const initialState = {
  userList: {
    items: [],
  },
  userListFilter: {
    nameFilter: '',
  },
  userDetail: {
    user: initialUser,
  }
}
