import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptionsArgs } from '@angular/http';
import { accessTokenKey, storageKey } from './constants';
import { LocalStorageService } from './local-storage.service';
import { Observable } from 'rxjs';
import {
  HttpClient,
  HttpEvent,
  HttpInterceptor,
  HttpRequest,
  HttpHandler
} from '@angular/common/http';

@Injectable()
export class HeaderInterceptor implements HttpInterceptor {
  constructor(private _storage: LocalStorageService) {}

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this._storage.get({ name: accessTokenKey }) || '';

    return next.handle(
      httpRequest.clone({
        headers: httpRequest.headers
          .set('Authorization', `Bearer ${token}`)
      })
    );
  }
}
