import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptionsArgs } from "@angular/http";
import { constants } from "./constants";
import { LocalStorageService } from "./local-storage.service";
import { Observable } from "rxjs";
import { HttpClient, HttpEvent, HttpInterceptor, HttpRequest, HttpHandler } from "@angular/common/http";

@Injectable()
export class HeaderInterceptor implements HttpInterceptor {
  constructor(private _storage: LocalStorageService) { }

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this._storage.get({ name: constants.ACCESS_TOKEN_KEY });
    //const culture = this._storage.get({ name: constants.CULTURE_KEY });
    
    return next.handle(httpRequest.clone({
      headers: httpRequest.headers
        .set('Authorization', `Bearer ${token}`)
        .set('Content-Type', 'application/json')
        .set('Accept-Language', 'fr-CA')
    }));
  }
}
