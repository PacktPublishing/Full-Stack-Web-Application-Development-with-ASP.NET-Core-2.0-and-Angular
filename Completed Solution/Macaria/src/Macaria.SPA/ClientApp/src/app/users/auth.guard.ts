import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';
import { LocalStorageService } from "../shared/local-storage.service";
import { Injectable } from "@angular/core";
import { constants } from "../shared/constants";
import { LoginRedirectService } from "./redirect.service";

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private _localStorageService: LocalStorageService,
    private _loginRedirectService: LoginRedirectService
  ) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    const token = this._localStorageService.get({ name: constants.ACCESS_TOKEN_KEY });

    if (token)
      return Observable.of(true);

    this._loginRedirectService.lastPath = state.url;
    this._loginRedirectService.redirectToLogin();

    return Observable.of(false);   
  }
}
