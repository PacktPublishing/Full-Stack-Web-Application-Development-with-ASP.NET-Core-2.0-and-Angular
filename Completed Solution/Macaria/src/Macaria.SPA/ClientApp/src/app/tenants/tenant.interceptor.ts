import { Injectable, Injector } from "@angular/core";
import { Http, Headers, RequestOptionsArgs } from "@angular/http";
import { Observable } from "rxjs";
import { HttpClient, HttpEvent, HttpInterceptor, HttpRequest, HttpHandler } from "@angular/common/http";
import { constants } from "../shared/constants";
import { LocalStorageService } from "../shared/local-storage.service";

@Injectable()
export class TenantInterceptor implements HttpInterceptor {
  private _localStorageService: LocalStorageService;

  constructor(injector: Injector) {
    this._localStorageService = injector.get(LocalStorageService);
  }

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {    
    return next.handle(httpRequest.clone({
      headers: httpRequest.headers.set('TenantId', this._localStorageService.get({ name: constants.TENANT_KEY }) || "")
    }));
  }
}
