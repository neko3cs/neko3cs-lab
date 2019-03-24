import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './main/main.component';
import { BookListComponent } from './book-list/book-list.component';
import { ArticleComponent } from './article/article.component';
import { ParamComponent } from './param/param.component';
import { DataComponent } from './data/data.component';
import { HatenaComponent } from './hatena/hatena.component';
import { HatenaResolver } from 'src/service/hatena/hatena-resolver';
import { HatenaService } from 'src/service/hatena/hatena.service';
import { PipeSampleComponent } from './pipe-sample/pipe-sample.component';
import { DirectiveSampleComponent } from './directive-sample/directive-sample.component';
import { TestSampleComponent } from './test-sample/test-sample.component';

const routes: Routes = [
  { path: 'book-list', component: BookListComponent },
  { path: 'hatena', component: HatenaComponent },
  { path: 'hatena/:url', component: HatenaComponent, resolve: { hatena: HatenaResolver } },
  { path: 'pipe', component: PipeSampleComponent },
  { path: 'directive', component: DirectiveSampleComponent },
  { path: 'test', component: TestSampleComponent },
  { path: 'article/:id', component: ArticleComponent },
  { path: 'param', component: ParamComponent },
  { path: 'data', component: DataComponent, data: { category: 'Angular' } },
  { path: '', component: MainComponent },
  { path: '**', redirectTo: '/' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [
    HatenaService,
    HatenaResolver
  ]
})
export class AppRoutingModule { }
