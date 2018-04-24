import { Injectable } from "@angular/core";
import { HttpClient, HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from "@angular/common/http";
import { Observable } from "rxjs";
import { LocalStorageService } from "../shared/local-storage.service";
import { constants } from "../shared/constants";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private _localStorageService: LocalStorageService) { }
  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest.clone({
      headers: httpRequest.headers.set('Authorization', `Bearer ${this._localStorageService.get({ name: constants.ACCESS_TOKEN_KEY })}`)
    }));
  }
}
