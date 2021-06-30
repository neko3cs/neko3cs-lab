import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component/app.component';
import { InMemoryWebApiModule } from 'angular-in-memory-web-api';
import { BooksData } from '../data/books-data.service';
import { MainComponent } from './main/main.component';
import { BookListComponent } from './book-list/book-list.component';
import { ArticleComponent } from './article/article.component';
import { ParamComponent } from './param/param.component';
import { DataComponent } from './data/data.component';
import { HatenaComponent } from './hatena/hatena.component';
import { Nl2brPipe } from '../pipe/nl2br/nl2br.pipe';
import { PipeSampleComponent } from './pipe-sample/pipe-sample.component';
import { ColoredDirective } from '../directive/colored/colored.directive';
import { DirectiveSampleComponent } from './directive-sample/directive-sample.component';
import { DeadlineDirective } from '../directive/deadline/deadline.directive';
import { TestSampleComponent } from './test-sample/test-sample.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    BookListComponent,
    ArticleComponent,
    ParamComponent,
    DataComponent,
    HatenaComponent,
    Nl2brPipe,
    PipeSampleComponent,
    ColoredDirective,
    DirectiveSampleComponent,
    DeadlineDirective,
    TestSampleComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientJsonpModule,
    InMemoryWebApiModule.forRoot(BooksData)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
