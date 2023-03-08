import { Component, OnDestroy, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserDetailUsecase } from '../usecases/user-detail.usecase';

@Component({
  templateUrl: './user-detail-page.component.html',
  styleUrls: ['./user-detail-page.component.css']
})
export class UserDetailPageComponent implements OnDestroy {
  user$ = this.userDetailUsecase.user$;

  private onDestroy$ = new EventEmitter();

  constructor(private route: ActivatedRoute, private userDetailUsecase: UserDetailUsecase) {
    this.userDetailUsecase.subscribeRouteChanges(this.route, this.onDestroy$);
  }

  ngOnDestroy() {
    this.onDestroy$.complete();
  }
}