import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HubClient } from './hub-client';
import { Observable } from 'rxjs';
import { LoginRedirectService } from './redirect.service';

@Injectable()
export class HubClientGuard implements CanActivate {
  constructor(private _hubClient: HubClient, private _loginRedirect: LoginRedirectService) {}

  public canActivate(): Promise<boolean> {
    return new Promise((resolve,reject) =>
      this._hubClient.connect().then(() => {
        resolve(true);
      }).catch((e) => {
        reject(e);
        this._loginRedirect.redirectToLogin();
      })
    );
  }
}
