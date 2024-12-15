import { ApplicationConfig } from '@angular/core';
import { Routes, provideRouter } from '@angular/router';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { AppComponent } from './app/components/app-component/app.component';
import { UserDetailPageComponent } from './app/components/user-detail-page/user-detail-page.component';

const routes: Routes = [
  { path: 'users/:userId', component: UserDetailPageComponent }
];
const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
  ]
};

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
