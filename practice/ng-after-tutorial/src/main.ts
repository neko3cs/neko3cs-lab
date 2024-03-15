import { ApplicationConfig } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, provideRouter } from '@angular/router';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
import { AppComponent } from './app/components/app-component/app.component';
import { UserDetailPageComponent } from './app/components/user-detail-page/user-detail-page.component';

const routes: Routes = [
  { path: 'users/:userId', component: UserDetailPageComponent }
];
const appConfig: ApplicationConfig = {
  providers: [
    CommonModule,
    BrowserModule,
    ReactiveFormsModule,
    provideHttpClient(),
    provideRouter(routes),
  ]
};


bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
