import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { accessTokenKey } from '../core/constants';
import { LocalStorageService } from '../core/local-storage.service';
import { LoginRedirectService } from './redirect.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private _localStorageService: LocalStorageService,
    private _loginRedirectService: LoginRedirectService
  ) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = this._localStorageService.get({ name: accessTokenKey });

    if (token) return true;

    this._loginRedirectService.lastPath = state.url;
    this._loginRedirectService.redirectToLogin();

    return false;
  }
}
