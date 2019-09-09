import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { takeUntil, map, distinctUntilChanged } from 'rxjs/operators';
import { UserApiService } from '../services/user-api/user-api.service';
import { Store } from '../services/store/store.service';

@Injectable({ providedIn: 'root' })
export class UserDetailUsecase {

    get user$() {
        return this.store.select(state => state.userDetail.user);
    }

    constructor(private userApi: UserApiService, private store: Store) { }

    subscribeRouteChanges(route: ActivatedRoute, untilObservable: Observable<any>) {
        route.params.pipe(
            takeUntil(untilObservable),
            map(params => params['userId']),
            distinctUntilChanged(),
        ).subscribe(userId => this.onUserIdChanged(userId));
    }

    private async onUserIdChanged(userId: string) {
        // リクエスト前に現在の状態をリセットする
        this.store.update(state => ({
            ...state,
            userDetail: {
                ...state.userDetail,
                user: null,
            }
        }));

        // APIを呼び出す
        const user = await this.userApi.getUserById(userId);

        // 状態を更新する
        this.store.update(state => ({
            ...state,
            userDetail: {
                ...state.userDetail,
                user,
            }
        }));
    }
}