import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { HatenaService } from './hatena.service';

@Injectable()
export class HatenaResolver implements Resolve<any> {

    constructor(private hatena: HatenaService) { }

    resolve(route: import("@angular/router").ActivatedRouteSnapshot, state: import("@angular/router").RouterStateSnapshot) {
        throw this.hatena.requestGet(route.params['url']);
    }
}
