import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpEventType,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalStorageService } from '../core/local-storage.service';
import { LoginRedirectService } from './redirect.service';
import { accessTokenKey } from '../core/constants';
import { tap } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private _localStorageService: LocalStorageService,
    private _loginRedirectService: LoginRedirectService
  ) {}
  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      tap(
        (httpEvent: HttpEvent<any>) => httpEvent,
        error => {
          if (error instanceof HttpErrorResponse && error.status === 401) {
            this._localStorageService.put({ name: accessTokenKey, value: null });
            this._loginRedirectService.redirectToLogin();
          }
        }
      )
    );
  }
}
