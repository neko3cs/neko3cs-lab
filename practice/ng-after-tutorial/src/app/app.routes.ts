import { Routes } from '@angular/router';
import { UserDetailPageComponent } from './user-detail-page/user-detail-page.component';


export const routes: Routes = [
  {
    path: 'users/:userId',
    component: UserDetailPageComponent
  }
];
