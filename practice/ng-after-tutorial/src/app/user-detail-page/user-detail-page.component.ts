import { Component, OnDestroy, EventEmitter, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { UserDetailUsecase } from '../usecases/user-detail.usecase';
import { Observable } from 'rxjs';
import { User } from '../user';

@Component({
  selector: 'user-detail-page',
  standalone: true,
  imports: [
    CommonModule,
  ],
  providers: [
    UserDetailUsecase,
  ],
  template: `
    <ng-container *ngIf="user$ | async as user; else userFetching">
      <h1>{{user.name}}</h1>

      <dl>
        <dt>Email</dt>
        <dd>{{ user.email }}</dd>
        <dt>Phone</dt>
        <dd>{{ user.phone }}</dd>
        <dt>Company</dt>
        <dd>{{ user.company.name }}</dd>
      </dl>

    </ng-container>

    <ng-template #userFetching>
      <div>Fetching...</div>
    </ng-template>
  `,
  styles: ``
})
export class UserDetailPageComponent implements OnDestroy {

  user$: Observable<User> = new Observable<User>();
  private onDestroy$ = new EventEmitter();

  constructor(
    @Inject(ActivatedRoute) private route: ActivatedRoute,
    @Inject(UserDetailUsecase) private userDetailUsecase: UserDetailUsecase
  ) {
    this.userDetailUsecase.user$.subscribe({
      next: (user: User) => {
        this.user$ = new Observable<User>(observable => observable.next(user));
      },
      error: error => {
        console.error(`ERROR: ${error}`);

      }
    });
    this.userDetailUsecase.subscribeRouteChanges(this.route, this.onDestroy$);
  }

  ngOnDestroy() {
    this.onDestroy$.complete();
  }
}
