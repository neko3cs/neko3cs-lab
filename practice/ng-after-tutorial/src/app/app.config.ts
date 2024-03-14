import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { routes } from './app.routes';
import { BrowserModule } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  providers: [
    CommonModule,
    BrowserModule,
    ReactiveFormsModule,
    provideHttpClient(),
    provideRouter(routes),
  ]
};
