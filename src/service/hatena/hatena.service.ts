import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HatenaService {
  baseUrl = 'http://b.hatena.ne.jp/entry/jsonlite';

  constructor(private http: HttpClient) { }

  requestGet(url: string): Observable<any> {
    let params = new HttpParams();
    params.set('url', url);

    return this.http.jsonp<any>(
      `${this.baseUrl}?${params.toString()}`,
      'callback'
    );
  }
}
